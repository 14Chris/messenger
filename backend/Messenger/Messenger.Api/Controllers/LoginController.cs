using Messenger.Api.DataModels;
using Messenger.Facade.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Messenger.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiKeyPolicy")]
    public class LoginController : BaseController
    {
        public LoginController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        // POST: User login informations to be authenticated in the system
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostLogin(LoginModel model)
        {
            try
            {
                ResponseObject response = _userService.Login(model.email, model.password);


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
    }
}
