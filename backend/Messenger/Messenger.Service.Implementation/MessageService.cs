﻿using Confluent.Kafka;
using Messenger.Database;
using Messenger.Facade.Models;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Service.Implementation
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings, ProducerConfig config) : base(serviceProvider, jwtSettings)
        { 
        }

        /// <summary>
        /// Create a message in conversation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ResponseObject> CreateMessage(int userId, Message message)
        {
            message.SenderId = userId;
            message.Date = DateTime.Now;

            Message result = await _messageRepository.CreateAsync(message);

            if(result == null)
            {
                return new ResponseObject(ResponseType.Error);
            }

            return new ResponseObject(ResponseType.Success, "", result);
        }

        /// <summary>
        /// Load more messages from conversation
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="conversationId"></param>
        /// <param name="lastMessageId"></param>
        /// <returns></returns>
        public ResponseObject LoadMoreMessagesFromConversation(int userId, int conversationId, int lastMessageId)
        {
            List<MessageModel> messages = _messageRepository.List()
                .Where(x => x.ConversationId == conversationId && x.Id < lastMessageId)
                .OrderByDescending(x=>x.Id)
                .Take(30)
                .Select(x=>new MessageModel()
                {
                    Id = x.Id,
                    SenderId = x.SenderId,
                    Text = x.Text,
                    Date = x.Date
                })
                .ToList();

            return new ResponseObject(ResponseType.Success, "", messages);
        }
    }
}
