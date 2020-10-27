using Messenger.Facade.Settings;
using Messenger.Repository.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Messenger.Service.Implementation
{
    public class BaseService
    {
        private IServiceProvider _serviceProvider;
        protected readonly JwtSettings _jwtSettings;

        public BaseService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings)
        {
            this._serviceProvider = serviceProvider;
            this._jwtSettings = jwtSettings.Value;
        }

        protected IUserRepository _userRepository
        {
            get { return _serviceProvider.GetRequiredService<IUserRepository>(); }
        }

        protected IMessageRepository _messageRepository
        {
            get { return _serviceProvider.GetRequiredService<IMessageRepository>(); }
        }

        protected IConversationRepository _conversationRepository
        {
            get { return _serviceProvider.GetRequiredService<IConversationRepository>(); }
        }

        protected IUserConversationRepository _userConversationRepository
        {
            get { return _serviceProvider.GetRequiredService<IUserConversationRepository>(); }
        }

        protected IUserRelationRepository _userRelationRepository
        {
            get { return _serviceProvider.GetRequiredService<IUserRelationRepository>(); }
        }

    }
}
