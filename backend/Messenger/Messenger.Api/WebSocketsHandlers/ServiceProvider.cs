using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Api.WebSocketsHandlers
{
    public class ServiceProvider
    {
        private IServiceProvider _serviceProvider;

        public ServiceProvider(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IUserService _userService
        {
            get { return _serviceProvider.GetRequiredService<IUserService>(); }
        }

        public IConversationService _conversationService
        {
            get { return _serviceProvider.GetRequiredService<IConversationService>(); }
        }

        public IFriendService _friendService
        {
            get { return _serviceProvider.GetRequiredService<IFriendService>(); }
        }

        public IMessageService _messageService
        {
            get { return _serviceProvider.GetRequiredService<IMessageService>(); }
        }

        public IUserConversationService _userConversationService
        {
            get { return _serviceProvider.GetRequiredService<IUserConversationService>(); }
        }

        public ICommunicationService _communicationService
        {
            get { return _serviceProvider.GetRequiredService<ICommunicationService>(); }
        }
    }
}
