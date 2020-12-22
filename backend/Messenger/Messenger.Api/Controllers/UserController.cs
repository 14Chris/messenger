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

        public const int CacheAgeSeconds = 60 * 60 * 24 * 30; // 30 days

        public UserController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // POST: Create a user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostUser(User user)
        {
            try
            {
                ResponseObject response = await _userService.CreateUser(user);

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

        // GET: Get user session by his auth token
        [HttpGet("session")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult GetSession()
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);
            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _userService.GetUserSession(id);

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

        // GET: Check if email is already used by an user
        [HttpGet("email_exists/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EmailAlreadyExists(string email)
        {
            try
            {
                ResponseObject response = _userService.EmailAlreadyExists(email);

                if (response.ResponseType == ResponseType.Error)
                {
                    return Conflict();
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // PUT: update password
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword(UpdatePasswordModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _userService.UpdateUserPassword(id, model.oldPassword, model.newPassword);

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

        // GET: send account activation email
        [HttpGet("account_activation/send/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> SendAccountActivationEmail(string email)
        {
            try
            {
                ResponseObject response = await _userService.ResendAccountActivationEmail(email);

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

        // GET: activate user account
        [HttpGet("account_activation/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> ActivateAccount(string token)
        {
            try
            {
                ResponseObject response = await _userService.ActivateAccount(token);

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

        // POST: to register token for forgot password
        [HttpPost("forgot_password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                ResponseObject response = await _userService.ForgotPassword(email);

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

        // GET: Validate reset password token
        [HttpGet("reset_password/{token}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateTokenPasswordReset(string token)
        {
            try
            {
                ResponseObject response = await _userService.ValidateTokenPasswordReset(token);

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

        // POST: Reset password
        [HttpPost("reset_password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                ResponseObject response = await _userService.ResetPassword(model.token, model.newPassword);

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

        // GET: User profile informations
        [HttpGet("{userId}/profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult GetUserProfile(int userId)
        {

            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _userService.GetUserProfile(userId);

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

        // GET: User session profile informations
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public IActionResult GetUserSessionProfile()
        {

            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = _userService.GetUserProfile(id);

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

        // GET: User profile picture
        [HttpGet("{id}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public IActionResult GetUserProfilePicture(int id)
        {
            try
            {
                ResponseObject response = _userService.GetUserProfilePicture(id);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                byte[] userPicture = response.Result as byte[];

                if (userPicture == null || userPicture.Length == 0)
                    return NotFound();

                Response.Headers["Cache-Control"] = $"public,max-age={CacheAgeSeconds}";
                Response.Headers["Access-Control-Allow-Origin"] = $"*";
                Response.Headers["Access-Control-Allow-Headers"] = $"*";

                return File(userPicture, "image/jpeg");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // HEAD: User profile picture
        [HttpHead("{id}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public IActionResult HeadUserProfilePicture(int id)
        {
            try
            {
                ResponseObject response = _userService.GetUserProfilePicture(id);

                if (response.ResponseType == ResponseType.Error)
                {
                    return StatusCode(500, response);
                }

                byte[] userPicture = response.Result as byte[];

                if (userPicture == null || userPicture.Length == 0)
                    return NotFound();

                return File(userPicture, "image/jpeg");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        // POST: Change user profile picture
        [HttpPost("{id}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> ChangeProfilePicture(ChangeProfilePictureModel model)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            if (!ok)
                return Unauthorized();

            try
            {
                ResponseObject response = await _userService.ChangeUserProfilePicture(id, model.picture);

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

        // DELETE: Delete user profile picture
        [HttpDelete("{userId}/picture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> DeleteProfilePicture(int userId)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            // Check if user is authenticated and if user id and request user id are same
            if (!ok || userId != id)
                return Unauthorized();

            try
            {
                ResponseObject response = await _userService.DeleteUserProfilePicture(userId);

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

        // PUT: Update user informations
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UserBasicModel user)
        {
            int id = -1;

            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            bool ok = int.TryParse(claimsIdentity.Name, out id);

            // Check if user is authenticated and if user id and request user id are same
            if (!ok || user.Id != id)
                return Unauthorized();

            try
            {
                ResponseObject response = await _userService.UpdateUserInformations(user);

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
