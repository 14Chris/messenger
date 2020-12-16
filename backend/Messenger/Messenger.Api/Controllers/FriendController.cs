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
    [Route("friends")]
    [ApiController]
    [Authorize(Policy = "ApiKeyPolicy")]
    public class FriendController : BaseController
    {
        public FriendController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        
        // GET: Get friend for a user by his session token
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetFriends()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _friendService.GetFriendsByUser(id);

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

        // GET: Get friend requests for a user by his session token
        [HttpGet("request")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetFriendRequests()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _friendService.GetFriendsRequestByUser(id);

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

        // GET: Search friends for a user by his session token and search term
        [HttpGet("search/{searchTerm}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetFriendsBySearch(string searchTerm)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _friendService.SearchFriendsByUser(id, searchTerm);

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

        // POST: Add a friend request
        [HttpPost("request")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostFriendRequest([FromBody]string friendEmail)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _friendService.AddFriend(id, friendEmail);

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

        // POST: Accept a friend's request
        [HttpPost("request/accept")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAcceptFriendRequest([FromBody] int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _friendService.AcceptFriendRequest(id, friendId);

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

        // Delete: Delete a friend's request
        [HttpDelete("request")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostDeleteFriendRequest([FromBody] int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _friendService.DeleteFriendRequest(id, friendId);

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

        // Delete: Delete a friend
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteFriend([FromBody] int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _friendService.DeleteFriend(id, friendId);

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
