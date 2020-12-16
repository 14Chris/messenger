using Messenger.Facade.Helpers;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Test.Api
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

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Test create user with same email
            Database.User userSameEmail = new Database.User();
            user.FirstName = "TestFirstName2";
            user.LastName = "TestLastName2";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ResponseObject resultSameEmail = await _userService.CreateUser(user);

            Assert.IsNotNull(resultSameEmail);
            Assert.IsTrue(resultSameEmail.ResponseType == ResponseType.Error);

        }

        [TestMethod]
        public async Task TestCreateUserWithoutPassword()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();

            //Test without password
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ResponseType == ResponseType.Error);

            Database.User user2 = new Database.User();
            user2.FirstName = "TestFirstName";
            user2.LastName = "TestLastName";
            user2.Email = "lenfant.chris@hotmail.fr";
            user2.Password = "";

            ResponseObject result2 = await _userService.CreateUser(user2);

            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.ResponseType == ResponseType.Error);
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

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);
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
            ResponseObject resultUpdate = await _userService.UpdateUserInformations(basicUser);

            Assert.IsNotNull(resultUpdate);
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

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);
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
            ResponseObject resultUpdate = await _userService.UpdateUserInformations(basicUser);

            Assert.IsNotNull(resultUpdate);
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

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);


            //Change user picture
            string base64Picture = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNk+A8AAQUBAScY42YAAAAASUVORK5CYII=";
            ResponseObject resultChangePicture = await _userService.ChangeUserProfilePicture(user.Id, base64Picture);

            Assert.IsNotNull(resultChangePicture);
            Assert.IsTrue(resultChangePicture.ResponseType == ResponseType.Success);

            //Get user profile picture
            ResponseObject resultGetPicture = _userService.GetUserProfilePicture(user.Id);

            Assert.IsNotNull(resultChangePicture);
            Assert.IsTrue(resultChangePicture.ResponseType == ResponseType.Success);

            byte[] userPicture = resultGetPicture.Result as byte[];

            string userPictureString = Convert.ToBase64String(userPicture);

            Assert.IsTrue(userPictureString.Equals(base64Picture));
        }

        [TestMethod]
        public async Task TestChangeUserPassword()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();

            string oldPassword = "ChrisChris11!";
            string newPassword = "TestPASSWord28&";

            //Create user
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = oldPassword;

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);

            //Change password user
            ResponseObject resultPasswordChange = await _userService.UpdateUserPassword(user.Id, oldPassword, newPassword);
            Assert.IsNotNull(resultPasswordChange);
            Assert.IsTrue(resultPasswordChange.ResponseType == ResponseType.Success);


            ResponseObject resultUser = _userService.GetUser(user.Id);

            Database.User updatedUser = resultUser.Result as Database.User;
            Assert.IsTrue(updatedUser.Password.Equals(SecurityHelper.HashPassword(newPassword)));

        }

        [TestMethod]
        public async Task TestChangeUserPasswordWithSamePassword()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();

            string oldPassword = "ChrisChris11!";

            //Create user
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = oldPassword;

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);

            //Change password user with same password
            ResponseObject resultPasswordChange = await _userService.UpdateUserPassword(user.Id, oldPassword, oldPassword);

            Assert.IsNotNull(resultPasswordChange);
            Assert.IsTrue(resultPasswordChange.ResponseType == ResponseType.Error);
            Assert.IsTrue(resultPasswordChange.Message == "SAME_NEW_PASSWORD");

        }

        [TestMethod]
        public async Task TestChangeUserPasswordWithWeakPassword()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();

            string oldPassword = "ChrisChris11!";
            string newPassword = "Chris";

            //Create user
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = oldPassword;

            ResponseObject result = await _userService.CreateUser(user);

            Assert.IsNotNull(result);

            //Change password user with weak password
            ResponseObject resultPasswordChange = await _userService.UpdateUserPassword(user.Id, oldPassword, newPassword);

            Assert.IsNotNull(resultPasswordChange);
            Assert.IsTrue(resultPasswordChange.ResponseType == ResponseType.Error);
            Assert.IsTrue(resultPasswordChange.Message == "NEW_PASSWORD_TOO_WEAK");

        }

    }
}

