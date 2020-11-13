﻿using Messenger.Database;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Messenger.Service.Implementation
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings) : base(serviceProvider, jwtSettings)
        {
        }

        /// <summary>
        /// Create a message in conversation
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> CreateMessage(int userId, Message message)
        {
            message.SenderId = userId;
            message.Date = DateTime.Now;

            Message result = await _messageRepository.CreateAsync(message);

            if(result == null)
            {
                return new ReturnApiObject(HttpStatusCode.BadRequest, ResponseType.Error);
            }

            return new ReturnApiObject(HttpStatusCode.Created, ResponseType.Success, "", result);
        }
    }
}