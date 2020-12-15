using Messenger.Facade;
using Messenger.Facade.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Api.WebSocketsHandlers
{

    /// <summary>
    /// Handler for the websockets request in the application
    /// </summary>
    public class ChatWebSocketHandler : IWebSocketHandler
    {
        private readonly RequestDelegate _next;


        public ChatWebSocketHandler(RequestDelegate next)
        {
            _next = next; 
        }

        /// <summary>
        /// Invoked when a websocket request is received
        /// </summary>
        /// <param name="context"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="webSocketStore"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider, WebSocketStore webSocketStore)
        {
            ServiceProvider _serviceProvider = new ServiceProvider(serviceProvider);

            //If it's not a websocket request
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            int id = -1;

            //Get the user identity from the token provided
            var claimsIdentity = context.User.Identity as ClaimsIdentity;

            
            bool ok = int.TryParse(claimsIdentity.Name, out id);
            if (!ok)
                return;

            CancellationToken ct = context.RequestAborted;
            WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync("access_token");

            webSocketStore.TryAddWebSocket(id, currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                //Read the data provided by websocket
                var response = await ReceiveStringAsync(currentSocket, ct);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }

                    continue;
                } 

                SocketRequestModel json = JsonConvert.DeserializeObject<SocketRequestModel>(response);

                //handle the request depending on the request type
                switch (json.type)
                {
                    //If the request is a new message sending
                    case "send_message":
                        _serviceProvider._communicationService.SendNewMessageNotification(id, json.data);
                        break;
                }
            }

            //Try to remove the user from the websocket store
            WebSocket dummy = webSocketStore.TryRemoveWebSocket(id);

            //CLose the socket connection
            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }

       
        /// <summary>
        /// Handle the websocket sended data
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
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

    }
}
