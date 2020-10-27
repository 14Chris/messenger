using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Messenger.Api.WebSocketsHandlers
{

    public class WebSocketAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;

            // web sockets cannot pass headers so we must take the access token from query param and
            // add it to the header before authentication middleware runs
            if (request.Headers.ContainsKey("sec-websocket-protocol"))
            {
                var protocols = request.Headers["sec-websocket-protocol"].ToString().Split(", ");
                var token = protocols[1];

                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Headers.Remove("sec-websocket-protocol");
            }

            await _next(httpContext);
        }
    }
}
