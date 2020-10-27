using Messenger.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Messenger.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private IServiceProvider _serviceProvider;

        public BaseController(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected IUserService _userService
        {
            get { return _serviceProvider.GetRequiredService<IUserService>(); }
        }

        protected IConversationService _conversationService
        {
            get { return _serviceProvider.GetRequiredService<IConversationService>(); }
        }

        protected IFriendService _friendService
        {
            get { return _serviceProvider.GetRequiredService<IFriendService>(); }
        }

        protected IMessageService _messageService
        {
            get { return _serviceProvider.GetRequiredService<IMessageService>(); }
        }

        protected IUserConversationService _userConversationService
        {
            get { return _serviceProvider.GetRequiredService<IUserConversationService>(); }
        }

    }
}

