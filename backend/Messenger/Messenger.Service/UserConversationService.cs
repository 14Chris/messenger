using Messenger.Database;
using Messenger.Facade.Settings;
using Messenger.Service.Implementation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public class UserConversationService : BaseService, IUserConversationService
    {
        public UserConversationService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings) : base(serviceProvider, jwtSettings)
        {
        }

        public List<int> GetConversationIdUsers(int idConversation)
        {
            List<int> result = _userConversationRepository.List().Where(x => x.ConversationId == idConversation).Select(x => x.UserId).ToList();

            return result;
        }

        public List<UserConversation> GetConversationUsers(int idConversation)
        {
            List<UserConversation> result = _userConversationRepository.List().Where(x => x.ConversationId == idConversation).ToList();

            return result;
        }

        public async Task<UserConversation> UpdateUserConversation(UserConversation conv)
        {
            UserConversation result = await _userConversationRepository.UpdateAsync(conv);

            return result;
        }
    }
}
