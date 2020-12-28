using System;
using System.Net;
using System.Threading.Tasks;
using Messenger.Database;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Messenger.Test.Api
{
    [TestClass]
    public class ConversationServiceTest : BaseTest
    {
        [TestMethod]
        public async Task TestCreateConversation()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();
            IConversationService _conversationService = _serviceProvider.GetRequiredService<IConversationService>();
            IFriendService _friendService = _serviceProvider.GetRequiredService<IFriendService>();

            //User 1
            User user = new User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            //Add user 1
            ResponseObject resultUser = await _userService.CreateUser(user);

            //User 2
            User user2 = new User();
            user2.FirstName = "TestFirstName2";
            user2.LastName = "TestLastName2";
            user2.Email = "lenfant.chris@gmail.com";
            user2.Password = "ChrisChris11!";

            //Add user 2
            ResponseObject resultUser2 = await _userService.CreateUser(user2);

            //Add users relation
            ResponseObject resultAddFriend = await _friendService.AddFriend(user.Id, user2.Email);

            //Accept friend
            ResponseObject resultAcceptFriend = await _friendService.AcceptFriendRequest(user.Id, user2.Id);


            MessageModel messageModel = new MessageModel()
            {
                Text = "test message",
                GifId = "",
                StickerId = "",
                StickerUrl = "",
                GifUrl = ""

            };

            //Add new conversation
            ResponseObject resultConversation = await _conversationService.CreateConversation(user.Id, new int[] { user2.Id }, messageModel);

            Assert.IsNotNull(resultConversation);
            Assert.IsTrue(resultConversation.ResponseType == ResponseType.Success);
        }

        [TestMethod]
        public async Task TestCreateConversationAlreadyExists()
        {
            IUserService _userService = _serviceProvider.GetRequiredService<IUserService>();
            IConversationService _conversationService = _serviceProvider.GetRequiredService<IConversationService>();
            IFriendService _friendService = _serviceProvider.GetRequiredService<IFriendService>();

            //User 1
            User user = new User();
            user.FirstName = "TestFirstName";
            user.LastName = "TestLastName";
            user.Email = "lenfant.chris@hotmail.fr";
            user.Password = "ChrisChris11!";

            //Add user 1
            ResponseObject resultUser = await _userService.CreateUser(user);

            //User 2
            User user2 = new User();
            user2.FirstName = "TestFirstName2";
            user2.LastName = "TestLastName2";
            user2.Email = "lenfant.chris@gmail.com";
            user2.Password = "ChrisChris11!";

            //Add user 2
            ResponseObject resultUser2 = await _userService.CreateUser(user2);

            //Add users relation
            ResponseObject resultAddFriend = await _friendService.AddFriend(user.Id, user2.Email);

            //Accept friend
            ResponseObject resultAcceptFriend = await _friendService.AcceptFriendRequest(user.Id, user2.Id);



            MessageModel messageModel = new MessageModel()
            {
                Text = "test message",
                GifId = "",
                StickerId = "",
                StickerUrl = "",
                GifUrl = ""

            };

            //Add new conversation
            ResponseObject resultConversation = await _conversationService.CreateConversation(user.Id, new int[] { user2.Id }, messageModel);

            Assert.IsNotNull(resultConversation);
            Assert.IsTrue(resultConversation.ResponseType == ResponseType.Success);


            MessageModel messageModel2 = new MessageModel()
            {
                Text = "test message with same friend",
                GifId = "",
                StickerId = "",
                StickerUrl = "",
                GifUrl = ""

            };

            //Add conversation with same friend
            ResponseObject resultConversationSameFriend = await _conversationService.CreateConversation(user.Id, new int[] { user2.Id }, messageModel2);

            Assert.IsNotNull(resultConversationSameFriend);
            Assert.IsTrue(resultConversationSameFriend.ResponseType == ResponseType.Error);
        }
    }
}
