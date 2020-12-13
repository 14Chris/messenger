using Messenger.Facade.Response;
using Microsoft.AspNetCore.Authorization;
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
        public ReturnApiObject GetFriends()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return _friendService.GetFriendsByUser(id);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: Get friend requests for a user by his session token
        [HttpGet("request")]
        [Authorize]
        public ReturnApiObject GetFriendRequests()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return _friendService.GetFriendsRequestByUser(id);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: Search friends for a user by his session token and search term
        [HttpGet("search/{searchTerm}")]
        [Authorize]
        public ReturnApiObject GetFriendsBySearch(string searchTerm)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return _friendService.SearchFriendsByUser(id, searchTerm);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // POST: Add a friend request
        [HttpPost("request")]
        [Authorize]
        public async Task<ReturnApiObject> PostFriendRequest([FromBody]string friendEmail)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _friendService.AddFriend(id, friendEmail);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // POST: Accept a friend's request
        [HttpPost("request/accept")]
        [Authorize]
        public async Task<ReturnApiObject> PostAcceptFriendRequest([FromBody] int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _friendService.AcceptFriendRequest(id, friendId);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // POST: Delete a friend's request
        [HttpPost("request/delete")]
        [Authorize]
        public async Task<ReturnApiObject> PostDeleteFriendRequest([FromBody] int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _friendService.DeleteFriendRequest(id, friendId);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // Delete: Delete a friend
        [HttpDelete]
        [Authorize]
        public async Task<ReturnApiObject> DeleteFriend([FromBody] int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _friendService.DeleteFriend(id, friendId);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }
    }
}
