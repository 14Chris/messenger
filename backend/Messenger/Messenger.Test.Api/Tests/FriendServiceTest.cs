using Messenger.Database;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Service.Implementation;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Messenger.Test.Api
{
    [TestClass]
    public class FriendServiceTest : BaseTest
    {
        [TestMethod]
        public async Task TestAddFriend()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();
            IFriendService _friendService = _serviceProvider.GetRequiredService<IFriendService>();

            //Create User 1
            Database.User user = new Database.User();
            user.FirstName = "UserFirstName";
            user.LastName = "UserLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject result = await _userService.CreateUser(user);

            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Create User 2
            Database.User friend = new Database.User();
            friend.FirstName = "FriendFirstName";
            friend.LastName = "FriendLastName";
            friend.Email = "lenfant.chris@gmail.com";
            friend.Password = "ChrisChris11!";

            ReturnApiObject resultFriend = await _userService.CreateUser(friend);

            Assert.IsTrue(resultFriend.ResponseType == ResponseType.Success);

            //Test add users relation

            ReturnApiObject resultAddFriend = await _friendService.AddFriend(user.Id, friend.Email);
            Assert.IsTrue(resultFriend.ResponseType == ResponseType.Success);
        }

        [TestMethod]
        public async Task TestAcceptFriend()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();
            IFriendService _friendService = _serviceProvider.GetRequiredService<IFriendService>();

            //Create User 1
            Database.User user = new Database.User();
            user.FirstName = "UserFirstName";
            user.LastName = "UserLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject result = await _userService.CreateUser(user);

            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Create User 2
            Database.User friend = new Database.User();
            friend.FirstName = "FriendFirstName";
            friend.LastName = "FriendLastName";
            friend.Email = "lenfant.chris@gmail.com";
            friend.Password = "ChrisChris11!";

            ReturnApiObject resultFriend = await _userService.CreateUser(friend);

            Assert.IsTrue(resultFriend.ResponseType == ResponseType.Success);

            //Test add users relation
            ReturnApiObject resultAddFriend = await _friendService.AddFriend(user.Id, friend.Email);
            Assert.IsTrue(resultFriend.ResponseType == ResponseType.Success);

            //Test accept friend
            ReturnApiObject resultAcceptFriend = await _friendService.AcceptFriendRequest(user.Id, friend.Id);
            Assert.IsTrue(resultAcceptFriend.ResponseType == ResponseType.Success);

            //Test accept not friend
            ReturnApiObject resultAcceptNotFriend = await _friendService.AcceptFriendRequest(user.Id, 200);
            Assert.IsTrue(resultAcceptNotFriend.ResponseType == ResponseType.Error);
        }

        [TestMethod]
        public async Task TestGetFriends()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();
            IFriendService _friendService = _serviceProvider.GetRequiredService<IFriendService>();

            //Create User 1
            Database.User user = new Database.User();
            user.FirstName = "UserFirstName";
            user.LastName = "UserLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            ReturnApiObject result = await _userService.CreateUser(user);

            Assert.IsTrue(result.ResponseType == ResponseType.Success);

            //Create User 2
            Database.User friend = new Database.User();
            friend.FirstName = "FriendFirstName";
            friend.LastName = "FriendLastName";
            friend.Email = "lenfant.chris@gmail.com";
            friend.Password = "ChrisChris11!";

            ReturnApiObject resultFriend = await _userService.CreateUser(friend);

            Assert.IsTrue(resultFriend.ResponseType == ResponseType.Success);

            //Test add users relation
            ReturnApiObject resultAddFriend = await _friendService.AddFriend(user.Id, friend.Email);
            Assert.IsTrue(resultFriend.ResponseType == ResponseType.Success);

            //Test accept friend
            ReturnApiObject resultAcceptFriend = await _friendService.AcceptFriendRequest(user.Id, friend.Id);
            Assert.IsTrue(resultAcceptFriend.ResponseType == ResponseType.Success);

            ReturnApiObject friendsList = _friendService.GetFriendsByUser(user.Id);
            Assert.IsTrue(resultAcceptFriend.ResponseType == ResponseType.Success);
            Assert.IsTrue(((List<UserBasicModel>)friendsList.Result).Select(x=>x.Id).Contains(friend.Id));

        }

    }
}
