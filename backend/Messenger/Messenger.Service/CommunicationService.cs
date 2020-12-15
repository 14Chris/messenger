using Kafka.Public;
using Kafka.Public.Loggers;
using Messenger.Database;
using Messenger.Facade;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Service.Implementation
{
    public class CommunicationService : BaseService, ICommunicationService
    {

        private readonly WebSocketStore _webSocketStore;

        public CommunicationService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings, WebSocketStore webSocketStore) : base(serviceProvider, jwtSettings)
        {
            this._webSocketStore = webSocketStore;
        }

        /// <summary>
        /// Handle websocket send new message request
        /// </summary>
        /// <param name="idUser"></param>
        /// <param name="requestData"></param>
        /// <param name="serviceProvider"></param>
        /// <param name="ct"></param>
        public async Task SendNewMessageNotification(int userId, dynamic requestData, CancellationToken ct = default(CancellationToken))
        {
            Message newMessage = new Message()
            {
                Text = requestData.text,
                ConversationId = requestData.conversation_id
            };

            ReturnApiObject result = await _serviceProvider.GetRequiredService<IMessageService>().CreateMessage(userId, newMessage);

            if (result != null && result.ResponseType == ResponseType.Success)
            {
                Message message = (Message)result.Result;

                //Get all user conversations 
                List<UserConversation> usersConv = _serviceProvider.GetRequiredService<IUserConversationService>().GetConversationUsers(((Message)result.Result).ConversationId);

                //Iterate to change conversation visibility when archived because a new message is added
                foreach (UserConversation userConv in usersConv)
                {
                    if (userConv.Visibility == ConversationVisibility.Archived)
                    {
                        userConv.Visibility = ConversationVisibility.Visible;

                        UserConversation resultConvUpdate = await _serviceProvider.GetRequiredService<IUserConversationService>().UpdateUserConversation(userConv);
                    }
                }

                //Get users Ids from conversation
                List<int> usersConvIds = usersConv.Select(x => x.UserId).ToList();

                foreach (int id in usersConvIds)
                {
                    WebSocket userSocket = _webSocketStore.GetWebSocketById(id);

                    if (userSocket == null || userSocket.State != WebSocketState.Open)
                        continue;

                    ConversationListItem conv = _serviceProvider.GetRequiredService<IConversationService>().GetConversationListItemById(message.ConversationId, userId);

                    dynamic objResult =
                    new
                    {
                        type = "MessageAdded",
                        data = new
                        {
                            conversation = conv,
                            message = message
                        }
                    };

                    string jsonResult = JsonConvert.SerializeObject(objResult);

                    await SendStringAsync(userSocket, jsonResult, ct);
                }

            }
        }

        public async Task SendConversationCreatedNotification(int conversationId, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                //Get all user conversations 
                List<UserConversation> usersConv = _serviceProvider.GetRequiredService<IUserConversationService>().GetConversationUsers(conversationId);

                //Iterate to change conversation visibility when archived because a new message is added
                foreach (UserConversation userConv in usersConv)
                {
                    if (userConv.Visibility == ConversationVisibility.Archived)
                    {
                        userConv.Visibility = ConversationVisibility.Visible;

                        UserConversation resultConvUpdate = await _serviceProvider.GetRequiredService<IUserConversationService>().UpdateUserConversation(userConv);
                    }
                }

                //Get users Ids from conversation
                List<int> usersConvIds = usersConv.Select(x => x.UserId).ToList();

                foreach (int id in usersConvIds)
                {
                    WebSocket userSocket = _webSocketStore.GetWebSocketById(id);

                    if (userSocket == null || userSocket.State != WebSocketState.Open)
                        continue;

                    ConversationListItem conv = _serviceProvider.GetRequiredService<IConversationService>().GetConversationListItemById(conversationId, id);

                    dynamic objResult =
                    new
                    {
                        type = "ConversationCreated",
                        data = new
                        {
                            conversation = conv,
                        }
                    };

                    string jsonResult = JsonConvert.SerializeObject(objResult);

                    await SendStringAsync(userSocket, jsonResult, ct);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
    }
}
