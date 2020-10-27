using Messenger.Api.WebSocketsHandlers.Models;
using Messenger.Database;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Api.WebSocketsHandlers
{

    public class ChatWebSocketHandler : IWebSocketHandler
    {
        //Keep connected users
        private ConcurrentDictionary<int, WebSocket> _sockets = new ConcurrentDictionary<int, WebSocket>();

        private readonly RequestDelegate _next;

        public ChatWebSocketHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IServiceProvider _serviceProvider)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            int id = -1;

            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);
            if (!ok)
                return;

            CancellationToken ct = context.RequestAborted;
            WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync("access_token");

            _sockets.TryAdd(id, currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                var response = await ReceiveStringAsync(currentSocket, ct);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }

                    continue;
                }

                ServiceProvider serviceProvider = new ServiceProvider(_serviceProvider);

                SocketRequestModel json = JsonConvert.DeserializeObject<SocketRequestModel>(response);

                switch (json.type)
                {
                    case "send_message":
                        SendMessageRequestHandler(id, json.data, serviceProvider);
                        break;
                }
            }

            WebSocket dummy;
            _sockets.TryRemove(id, out dummy);

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }

        private Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        /// <summary>
        /// Handle websocket send new message request
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="requestData"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="ct"></param>
        private async void SendMessageRequestHandler(int idUser, dynamic requestData, ServiceProvider serviceProvider, CancellationToken ct = default(CancellationToken))
        {
            Message newMessage = new Message()
            {
                Text = requestData.text,
                ConversationId = requestData.conversation_id
            };

            ReturnApiObject result = await serviceProvider._messageService.CreateMessage(idUser, newMessage);    

            if (result != null && result.ResponseType == ResponseType.Success)
            {
                Message message = (Message)result.Result;

                ConversationListItem conv = serviceProvider._conversationService.GetConversationListItemById(message.ConversationId, idUser);

                dynamic objResult =
                new {
                    conversation = conv,
                    message = message
                };

                string jsonResult = JsonConvert.SerializeObject(objResult);

                //Get all user conversations 
                List<UserConversation> usersConv = serviceProvider._userConversationService.GetConversationUsers(((Message)result.Result).ConversationId);

                //Iterate to change conversation visibility because a new message is added
                foreach(UserConversation userConv in usersConv)
                {
                    if(userConv.Visibility == ConversationVisibility.NotVisible)
                    {
                        userConv.Visibility = ConversationVisibility.Visible;

                        UserConversation resultConvUpdate = await serviceProvider._userConversationService.UpdateUserConversation(userConv);
                    }
                    
                }

                //Get users Ids from conversation
                List<int> usersConvId = usersConv.Select(x => x.Id).ToList();

                List<WebSocket> userSockets = _sockets.Where(x => usersConvId.Contains(x.Key)).Select(x => x.Value).ToList();

                foreach (var socket in userSockets)
                {
                    if (socket.State != WebSocketState.Open)
                    {
                        continue;
                    }

                    await SendStringAsync(socket, jsonResult, ct);
                }

            }
        }


    }
}
