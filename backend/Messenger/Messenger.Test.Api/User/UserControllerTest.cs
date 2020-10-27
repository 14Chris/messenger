using Messenger.Facade.Response;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Test.Api.User
{
    [TestClass]
    public class UserControllerTest : BaseTest
    {

        [TestMethod]
        public async Task CreateUser()
        {
            Messenger.Api.Controllers.UserController _userApiController = _serviceProvider.GetService<Messenger.Api.Controllers.UserController>();

            //Test
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject result = await _userApiController.PostUser(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.HttpStatus == HttpStatusCode.Created);
            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Test brand with same email
            Database.User userSameEmail = new Database.User();
            user.FirstName = "TestFirstName2";
            user.LastName = "TestLastName2";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject resultSameEmail = await _userApiController.PostUser(user);

            Assert.IsNotNull(resultSameEmail, resultSameEmail.HttpStatus.ToString());
            Assert.IsTrue(resultSameEmail.HttpStatus == HttpStatusCode.BadRequest, resultSameEmail.HttpStatus.ToString());
            Assert.IsTrue(resultSameEmail.ResponseType == ResponseType.Error);

        }

        //[TestMethod]
        //public async Task UpdateUser()
        //{
        //    Messenger.Api.Controllers.UserController _userApiController = _serviceProvider.GetService<Messenger.Api.Controllers.UserController>();

        //    //Create user
        //    Database.User user = new Database.User();
        //    user.FirstName = "TestFirstName";
        //    user.LastName = "TestLastName";
        //    user.Email = "lenfant.chris@hotmail.fr";
        //    user.Password = "ChrisChris11!";

        //    ReturnApiObject result = await _userApiController.PostUser(user);

        //    Assert.IsNotNull(result);
        //    Assert.IsTrue(result.HttpStatus == HttpStatusCode.Created);
        //    Assert.IsTrue(result.ResponseType == ResponseType.Success);

        //    //Create user
        //    user.FirstName = "TestUpdateName";

        //    ReturnApiObject resultUpdate = await _userApiController.(user);

        //    Assert.IsNotNull(resultUpdate);
        //    Assert.IsTrue(resultUpdate.HttpStatus == HttpStatusCode.OK);
        //    Assert.IsTrue(resultUpdate.ResponseType == ResponseType.Success);

        //}
    }
}
