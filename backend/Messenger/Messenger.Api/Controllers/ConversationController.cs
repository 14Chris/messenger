using Messenger.Facade.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConversationController : BaseController
    {
        public ConversationController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // GET: Get conversations for a user by his session token
        [HttpGet("user")]
        [Authorize]
        public ReturnApiObject GetConversationByUser()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return _conversationService.GetConversationsByUser(id);
            }
            catch (Exception ex)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: Get conversations by his id
        [HttpGet("{convId}")]
        [Authorize]
        public ReturnApiObject GetConversationById(int convId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return _conversationService.GetConversationById(convId, id);
            }
            catch (Exception ex)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // POST: Add new conversation
        [HttpPost]
        [Authorize]
        public async Task<ReturnApiObject> PostConversation(NewConversationModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _conversationService.CreateConversation(id, model.friends, model.texte);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // POST: Add new conversation
        [HttpPost("exists")]
        [Authorize]
        public ReturnApiObject PostConversationExists([FromBody]List<int> users)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            users.Add(id);

            try
            {
                return _conversationService.GetConversationExistsByUsers(users.ToArray());
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }



    }
}
