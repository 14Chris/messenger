using Messenger.Facade.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiKeyPolicy")]
    public class ConversationController : BaseController
    {
        public ConversationController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // GET: Get conversations for a user by his session token
        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetConversationByUser()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _conversationService.GetConversationsByUser(id);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // GET: Get conversations by his id
        [HttpGet("{convId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetConversationById(int convId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _conversationService.GetConversationById(convId, id);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // GET: Get conversations by his id
        [HttpGet("{convId}/messages/{lastMessageId}/more")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetConversationMoreMessages(int convId, int lastMessageId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _messageService.LoadMoreMessagesFromConversation(id, convId, lastMessageId);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // GET: Get conversations detail by his id
        [HttpGet("{convId}/detail")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetConversationDetailById(int convId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _conversationService.GetConversationDetailById(convId, id);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        // POST: Add new conversation
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostConversation(NewConversationModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _conversationService.CreateConversation(id, model.friends, model.texte);

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

        // POST: Verify if conversation already exists with the users in body
        [HttpPost("exists")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult PostConversationExists([FromBody] List<int> users)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            users.Add(id);

            try
            {
                ResponseObject response = _conversationService.GetConversationExistsByUsers(users.ToArray());

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                //NO conversation already exists
                if (response.Result == null)
                {
                    return Ok();
                }

                //conversation already exists
                return Conflict(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT: Archive user conversation
        [HttpPut("{convId}/archive")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutConversationArchive(int convId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _conversationService.ArchiveConversation(convId, id);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
