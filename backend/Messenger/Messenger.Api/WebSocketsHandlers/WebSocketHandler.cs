﻿using Messenger.Facade;
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

    public class ChatWebSocketHandler : IWebSocketHandler
    {
        private readonly RequestDelegate _next;


        public ChatWebSocketHandler(RequestDelegate next)
        {
            _next = next; 
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider, WebSocketStore webSocketStore)
        {
            ServiceProvider _serviceProvider = new ServiceProvider(serviceProvider);

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

            webSocketStore.TryAddWebSocket(id, currentSocket);

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

                SocketRequestModel json = JsonConvert.DeserializeObject<SocketRequestModel>(response);

                switch (json.type)
                {
                    case "send_message":
                        _serviceProvider._communicationService.SendNewMessageNotification(id, json.data);
                        break;
                }
            }

            WebSocket dummy = webSocketStore.TryRemoveWebSocket(id);

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
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

    }
}
