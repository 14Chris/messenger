using Messenger.Database;
using Messenger.Facade.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.Api.Controllers
{
    [Route("messages")]
    [ApiController]
    [Authorize(Policy = "ApiKeyPolicy")]
    public class MessageController : BaseController
    {
        public MessageController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // POST: Add new message
        [HttpPost]
        [Authorize]
        public async Task<ReturnApiObject> PostConversation(Message message)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _messageService.CreateMessage(id, message);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }
    }
}
