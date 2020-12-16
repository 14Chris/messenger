using Messenger.Database;
using Messenger.Facade.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostMessage(Message message)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _messageService.CreateMessage(id, message);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                return Created("", response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
