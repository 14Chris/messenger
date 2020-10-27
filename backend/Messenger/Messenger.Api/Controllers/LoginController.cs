using Messenger.Api.DataModels;
using Messenger.Facade.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Messenger.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : BaseController
    {
        public LoginController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ReturnApiObject PostLogin(LoginModel model)
        {
            try
            {
                return _userService.Login(model.email, model.password);
            }
            catch (Exception ex)
            {
                return new ReturnApiObject(HttpStatusCode.InternalServerError);
            }
        }
    }
}
