using Messenger.Api.ApiModels;
using Messenger.Database;
using Messenger.EmailSending.Models;
using Messenger.Facade.Models;
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
    [Route("users")]
    [ApiController]
    [Authorize(Policy = "ApiKeyPolicy")]
    public class UserController : BaseController
    {
        public UserController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // POST: Create a user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ReturnApiObject> PostUser(User user)
        {
            try
            {
                return await _userService.CreateUser(user);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError);
            }
        }

        // GET: Get user session by his auth token
        [HttpGet("session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public ReturnApiObject GetSession()
        {
            try
            {
                int id = -1;

                var claimsIdentity = this.User.Identity as ClaimsIdentity;

                bool ok = int.TryParse(claimsIdentity.Name, out id);
                if (!ok)
                    return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

                return _userService.GetUserSession(id);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: Check if email is already used by an user
        [HttpGet("email_exists/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public ReturnApiObject EmailAlreadyExists(string email)
        {
            try
            {
                return _userService.EmailAlreadyExists(email);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }
         
        // PUT: update password
        [HttpPut("password")]
        [Authorize]
        public async Task<ReturnApiObject> UpdateUserPassword(UpdatePasswordModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _userService.UpdateUserPassword(id, model.oldPassword, model.newPassword);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }

        }

        // GET: send account activation email
        [HttpGet("account_activation/send/{email}")]
        [AllowAnonymous]
        public async Task<ReturnApiObject> SendAccountActivationEmail(string email)
        {
            try
            {
                return await _userService.ResendAccountActivationEmail(email);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: activate user account
        [HttpGet("account_activation/{token}")]
        [AllowAnonymous]
        public async Task<ReturnApiObject> ActivateAccount(string token)
        {
            try
            {
                return await _userService.ActivateAccount(token);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // POST: to register token for forgot password
        [HttpPost("forgot_password")]
        [AllowAnonymous]
        public async Task<ReturnApiObject> ForgotPassword([FromBody] string email)
        {

            try
            {
                return await _userService.ForgotPassword(email);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: Validate reset password token
        [HttpGet("reset_password/{token}")]
        [AllowAnonymous]
        public async Task<ReturnApiObject> ValidateTokenPasswordReset(string token)
        {
            try
            {
                return await _userService.ValidateTokenPasswordReset(token);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }

        }

        // POST: Reset password
        [HttpPost("reset_password")]
        [AllowAnonymous]
        public async Task<ReturnApiObject> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                return await _userService.ResetPassword(model.token, model.newPassword);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: User profile informations
        [HttpGet("{id}/profile")]
        [Authorize]
        public ReturnApiObject GetUserProfile(int id)
        {
            try
            {
                return _userService.GetUserProfile(id);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: User session profile informations
        [HttpGet("profile")]
        [Authorize]
        public ReturnApiObject GetUserSessionProfile()
        {

            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return _userService.GetUserProfile(id);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // GET: User profile picture
        [HttpGet("{id}/picture")]
        //[AllowAnonymous]
        public IActionResult GetUserProfilePicture(int id)
        {
            try
            {
                byte[] userPicture = _userService.GetUserProfilePicture(id);

                if (userPicture == null || userPicture.Length == 0)
                    return NotFound();

                return File(userPicture, "image/jpeg");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // HEAD: User profile picture
        [HttpHead("{id}/picture")]
        //[Authorize]
        public IActionResult HeadUserProfilePicture(int id)
        {
            try
            {
                byte[] userPicture = _userService.GetUserProfilePicture(id);

                if (userPicture == null || userPicture.Length == 0)
                    return NotFound();

                return File(userPicture, "image/jpeg");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST: Chnage user profile picture
        [HttpPost("{id}/picture")]
        [Authorize]
        public async Task<ReturnApiObject> ChangeProfilePicture(ChangeProfilePictureModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _userService.ChangeUserProfilePicture(id, model.picture);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // DELETE: Delete user profile picture
        [HttpDelete("{id}/picture")]
        [Authorize]
        public async Task<ReturnApiObject> DeleteProfilePicture(ChangeProfilePictureModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _userService.DeleteUserProfilePicture(id);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }

        // PUT: Update user informations
        [HttpPut]
        [Authorize]
        public async Task<ReturnApiObject> UpdateUser(UserBasicModel user)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok || user.Id != id)
                return new ReturnApiObject(HttpStatusCode.Unauthorized, ResponseType.Error);

            try
            {
                return await _userService.UpdateUserInformations(user);
            }
            catch (Exception)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError, ResponseType.Error);
            }
        }
    }
}
