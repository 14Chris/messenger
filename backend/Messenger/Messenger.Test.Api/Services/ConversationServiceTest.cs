using System;
using System.Net;
using System.Threading.Tasks;
using Messenger.Database;
using Messenger.Facade.Response;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Messenger.Test.Api.User
{
    [TestClass]
    public class ConversationServiceTest: BaseTest
    {
        [TestMethod]
        public async Task TestCreateConversation()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();
            IConversationService _conversationService = _serviceProvider.GetRequiredService<IConversationService>();
            IFriendService _friendService = _serviceProvider.GetRequiredService<IFriendService>();

            //User 1
            Database.User user = new Database.User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            //Add user 1
            ReturnApiObject resultUser = await _userService.CreateUser(user);

            //User 2
            Database.User user2 = new Database.User();
            user2.FirstName = "TestFirstName2";
            user2.LastName = "TestLastName2";
            user2.Email = "lenfant.chris@gmail.com";
            user2.Password = "ChrisChris11!";

            //Add user 2
            ReturnApiObject resultUser2 = await _userService.CreateUser(user2);

            //Add users relation
            ReturnApiObject resultAddFriend = await _friendService.AddFriend(user.Id, user2.Email);

            //Accept friend
            ReturnApiObject resultAcceptFriend = await _friendService.AcceptFriendRequest(user.Id, user2.Id);

            //Add new conversation
            ReturnApiObject resultConversation = await _conversationService.CreateConversation(user.Id, new int[] { user2.Id }, "test message");

            Assert.IsNotNull(resultConversation);
            Assert.IsTrue(resultConversation.HttpStatus == HttpStatusCode.Created);
            Assert.IsTrue(resultConversation.ResponseType == ResponseType.Success);
        }
    }
}
