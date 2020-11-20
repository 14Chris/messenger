using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Service.Implementation;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

            UserBasicModel basicUser = new UserBasicModel()
            {
                Id = user.Id,
                FirstName = newName,
                LastName = user.LastName,
                Email = user.Email,

            };

            //Update user informations
            ReturnApiObject resultUpdate = await _userService.UpdateUserInformations(basicUser);

            Assert.IsNotNull(resultUpdate);
            Assert.IsTrue(resultUpdate.HttpStatus == HttpStatusCode.OK);
            Assert.IsTrue(resultUpdate.ResponseType == ResponseType.Success);
            Assert.IsTrue(((Database.User)resultUpdate.Result).FirstName == newName);

        }

        [TestMethod]
        public async Task TestUpdateUserWithNewMail()
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

            //Change user email
            string newName = "TestUpdateName";
            string newEmail = "lenfant.chris@gmail.com";

            UserBasicModel basicUser = new UserBasicModel()
            {
                Id = user.Id,
                FirstName = newName,
                LastName = user.LastName,
                Email = newEmail,

            };

            //Update user informations
            ReturnApiObject resultUpdate = await _userService.UpdateUserInformations(basicUser);

            Assert.IsNotNull(resultUpdate);
            Assert.IsTrue(resultUpdate.HttpStatus == HttpStatusCode.OK);
            Assert.IsTrue(resultUpdate.ResponseType == ResponseType.Success);
            Assert.IsTrue(((Database.User)resultUpdate.Result).FirstName == newName);
            Assert.IsTrue(((Database.User)resultUpdate.Result).Email == user.Email);

        }


        [TestMethod]
        public async Task TestAddUserProfilePicture()
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


            //Change user picture
            string base64Picture = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";
            ReturnApiObject resultChangePicture = await _userService.ChangeUserProfilePicture(user.Id, base64Picture);

            Assert.IsNotNull(resultChangePicture);
            Assert.IsTrue(resultChangePicture.HttpStatus == HttpStatusCode.OK);
            Assert.IsTrue(resultChangePicture.ResponseType == ResponseType.Success);

            //Get user profile picture
            byte[] resultGetPicture =  _userService.GetUserProfilePicture(user.Id);
            string userPictureString = Convert.ToBase64String(resultGetPicture);

            Assert.IsTrue(userPictureString.Equals(base64Picture));
        }
    }
}
