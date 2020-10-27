using Messenger.Api.Controllers;
using Messenger.Repository.Implementation;
using Messenger.Service.Implementation;
using Messenger.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Messenger.Api
{
    public class MessengerRegister
    {
        public static void Configuration(IServiceCollection services)
        {
            Assembly assembly = typeof(BaseController).GetTypeInfo().Assembly;
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0).AddApplicationPart(assembly).AddControllersAsServices();

            ConfigurationServices(services);
            ConfigurationRepositories(services);
        }

        public static void ConfigurationServices(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IConversationService, ConversationService>();
            services.AddTransient<IFriendService, FriendService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IUserConversationService, UserConversationService>();
        }

        public static void ConfigurationRepositories(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IConversationRepository, ConversationRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserConversationRepository, UserConversationRepository>();
            services.AddTransient<IUserRelationRepository, UserRelationRepository>();
        }
    }
}