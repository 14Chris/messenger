using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Messenger.Facade
{
    public class WebSocketStore
    {
        //Keep connected users
        private ConcurrentDictionary<int, WebSocket> _sockets { get; set; }

        public WebSocketStore()
        {
            _sockets = new ConcurrentDictionary<int, WebSocket>();
        }

        public WebSocket GetWebSocketById(int id)
        {
           return _sockets.Where(x => id == x.Key).Select(x => x.Value).SingleOrDefault();
        }

        /// <summary>
        /// Try to add or replace user opened websocket
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        public bool TryAddWebSocket(int userId, WebSocket webSocket)
        {
            return _sockets.TryAdd(userId, webSocket);
        }

        /// <summary>
        /// Try to remove socket from opened user websockets
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WebSocket TryRemoveWebSocket(int userId)
        {
            WebSocket removedWebSocket;

            _sockets.TryRemove(userId, out removedWebSocket);

            return removedWebSocket;
        }


    }
}
