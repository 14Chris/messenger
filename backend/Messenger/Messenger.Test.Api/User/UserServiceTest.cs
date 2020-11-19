using Messenger.Facade.Response;
using Messenger.Service.Implementation;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Test.Api.User
{
    [TestClass]
    public class UserServiceTest : BaseTest
    {

        [TestMethod]
        public async Task TestCreateUser()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();

            //Test
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.HttpStatus == HttpStatusCode.Created);
            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Test brand with same email
            Database.User userSameEmail = new Database.User();
            user.FirstName = "TestFirstName2";
            user.LastName = "TestLastName2";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject resultSameEmail = await _userService.CreateUser(user);

            Assert.IsNotNull(resultSameEmail, resultSameEmail.HttpStatus.ToString());
            Assert.IsTrue(resultSameEmail.HttpStatus == HttpStatusCode.BadRequest, resultSameEmail.HttpStatus.ToString());
            Assert.IsTrue(resultSameEmail.ResponseType == ResponseType.Error);

        }

        [TestMethod]
        public async Task TestUpdateUser()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();

            //Create user
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.HttpStatus == HttpStatusCode.Created);
            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Create user
            string newName = "TestUpdateName";
            user.FirstName = newName;

            ReturnApiObject resultUpdate = await _userService.UpdateUserInformations(user);

            Assert.IsNotNull(resultUpdate);
            Assert.IsTrue(resultUpdate.HttpStatus == HttpStatusCode.OK);
            Assert.IsTrue(resultUpdate.ResponseType == ResponseType.Success);
            Assert.IsTrue(((Database.User)resultUpdate.Result).FirstName == newName);

        }
    }
}
