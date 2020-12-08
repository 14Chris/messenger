using Messenger.Database;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Implementation
{
    public class ConversationService : BaseService, IConversationService
    {
        public ConversationService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings) : base(serviceProvider, jwtSettings)
        {
        }

        /// <summary>
        /// Create a new conversation
        /// </summary>
        /// <param name="idCreator"></param>
        /// <param name="friends"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> CreateConversation(int idCreator, int[] friends, string message)
        {
            List<int> usersConv = friends.ToList();
            usersConv.Add(idCreator);

            bool convExists = GetConversationsByUsers(usersConv.ToArray()) != null;

            if (convExists)
            {
                return new ReturnApiObject(HttpStatusCode.Conflict, ResponseType.Error, "CONVERSATION_ALREADY_EXISTS", null);
            }

            Conversation conversation = new Conversation();
            conversation.CreatorId = idCreator;

            conversation.Conversations = new List<UserConversation>();

            List<User> users = _userRepository.List().Where(x => usersConv.Contains(x.Id)).ToList();

            UserConversation userConversationCreator = new UserConversation()
            {
                UserId = idCreator,
                Name = string.Join(", ", users.Where(x => x.Id != idCreator).Select(x => x.FirstName + " " + x.LastName).ToArray()),
                Visibility = ConversationVisibility.Visible,
            };

            conversation.Conversations.Add(userConversationCreator);

            foreach (int idFriend in friends)
            {
                UserConversation userConversationFriend = new UserConversation()
                {
                    UserId = idFriend,
                    Name = string.Join(", ", users.Where(x => x.Id != idFriend).Select(x => x.FirstName + " " + x.LastName).ToArray()),
                    Visibility = ConversationVisibility.Visible,
                };
                conversation.Conversations.Add(userConversationFriend);
            }

            Message newMessage = new Message()
            {
                SenderId = idCreator,
                Date = DateTime.Now,
                Text = message,
            };

            conversation.Messages = new List<Message>();
            conversation.Messages.Add(newMessage);

            Conversation result = await _conversationRepository.CreateAsync(conversation);

            if (result == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            return new ReturnApiObject(HttpStatusCode.Created, ResponseType.Success, "", conversation);
        }

        /// <summary>
        /// Get conversation by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnApiObject GetConversationById(int id, int userId)
        {
            Conversation conversation = _conversationRepository.List().Where(x => x.Id == id).SingleOrDefault();
         
            if(conversation == null)
            {
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Error, "CONVERSATION_NOT_FOUND", null);
            }

            UserConversation userConversation = _userConversationRepository.List().Where(x => x.UserId == userId && x.ConversationId == id).SingleOrDefault();

            if (conversation == null)
            {
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Error, "USER_CONVERSATION_NOT_FOUND", null);
            }

            ConversationModel model = new ConversationModel()
            {
                Id = conversation.Id,
                Name = userConversation.Name,
                Messages = _messageRepository.List().Where(x=>x.ConversationId == id).OrderByDescending(x=>x.Date).Take(20).Select(x=>new MessageModel()
                {
                    Id = x.Id,
                    SenderId = x.SenderId,
                    Text = x.Text,
                    Date = x.Date
                }).ToList()

            };

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", model);
        }

        /// <summary>
        /// Get all users conversations
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnApiObject GetConversationsByUser(int id)
        {
            DateTime dateTime = new DateTime();
            
            List<ConversationListItem> conversations = _userConversationRepository.List().Where(x => x.UserId == id && x.Visibility == ConversationVisibility.Visible)
                .Select(x =>
                    new ConversationListItem
                    {
                        Id = x.Conversation.Id,
                        Name = x.Name,
                        LastMessage = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault() != null ? x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Text : "",
                        LastMessageDate = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault() != null ? x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Date : dateTime,
                        FriendsIds = x.Conversation.Conversations.Where(x=>x.UserId != id).Select(a=>x.UserId).ToList(),
                        LastMessageSender = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault() != null ? new UserBasicModel()
                        {
                            Id = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Sender.Id,
                            FirstName = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Sender.FirstName,
                            LastName = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Sender.LastName,
                            Email = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Sender.Email,
                        } 
                        : null,
                    }
                ).ToList();

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", conversations);
        }

        /// <summary>
        /// Get the conversation by a array of users
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public ReturnApiObject GetConversationExistsByUsers(int[] users)
        {
            ConversationModel conversation = GetConversationsByUsers(users);
            
            if (conversation == null)
            {
                return new ReturnApiObject(HttpStatusCode.NotFound, ResponseType.Success);
            }

            return new ReturnApiObject(HttpStatusCode.OK, ResponseType.Success, "", conversation);
        }


        private ConversationModel GetConversationsByUsers(int[] users)
        {
            List<Conversation> conversations = _conversationRepository.List()
                .Include(x=>x.Conversations)
                .Include(x=>x.Messages)
                .Where(x => x.Conversations.Select(a=>a.UserId).Contains((users[0])))
                .ToList();

            ConversationModel conversation = conversations
                .Where(x => x.Conversations.Select(a => a.UserId).OrderBy(x => x).SequenceEqual(users.OrderBy(x=>x)))
                .Select(x=>new ConversationModel()
                {
                    Id = x.Id,
                    Messages = x.Messages.Select(m=>new MessageModel()
                    {
                        Id = m.Id,
                        SenderId = m.SenderId,
                        Text = m.Text,
                        Date = m.Date
                    }).ToList(),
                    Name = ""
                })
                .SingleOrDefault();

            return conversation;
        }

        /// <summary>
        /// Get conversation list item by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ConversationListItem GetConversationListItemById(int id, int userId)
        {
            DateTime dateTime = new DateTime();


            ConversationListItem model = _userConversationRepository.List().Where(x => x.UserId == userId && x.ConversationId == id).Select(x => new ConversationListItem
            {
                Id = x.Conversation.Id,
                Name = x.Name,
                LastMessage = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault() != null ? x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Text : "",
                LastMessageDate = x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault() != null ? x.Conversation.Messages.OrderByDescending(x => x.Date).FirstOrDefault().Date : dateTime
            }).SingleOrDefault();

            return model;
        }
    }
}
