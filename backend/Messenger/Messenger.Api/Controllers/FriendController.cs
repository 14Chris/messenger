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
        public async Task<ReturnApiObject> PostFriendRequest([FromBody]int friendId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _friendService.AddFriend(id, friendId);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        //// DELETE: friend
        //[HttpDelete]
        //[Authorize]
        //public async Task<ReturnApiObject> DeleteFriend([FromBody] int friendId)
        //{
        //    int id = -1;

        //    var claimsIdentity = this.User.Identity as ClaimsIdentity;

        //    bool ok = int.TryParse(claimsIdentity.Name, out id);

        //    if (!ok)
        //        return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

        //    try
        //    {
        //        return await _friendService.AddFriend(id, friendId);
        //    }
        //    catch (Exception)
        //    {
        //        return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
        //    }
        //}

        // POST: Add a friend request
        //[HttpPost("accept")]
        //[Authorize]
        //public async Task<ReturnApiObject> PostFriendAcceptRequest([FromBody] int friendId)
        //{
        //    int id = -1;

        //    var claimsIdentity = this.User.Identity as ClaimsIdentity;

        //    bool ok = int.TryParse(claimsIdentity.Name, out id);

        //    if (!ok)
        //        return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

        //    try
        //    {
        //        return await _friendService.AddFriend(id, friendId);
        //    }
        //    catch (Exception)
        //    {
        //        return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
        //    }
        //}
    }
}
