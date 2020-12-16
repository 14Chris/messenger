using Messenger.Database;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Implementation;
using Messenger.Service.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Implementation
{
    public class UserConversationService : BaseService, IUserConversationService
    {
        public UserConversationService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings) : base(serviceProvider, jwtSettings)
        {
        }

        public ResponseObject GetConversationIdUsers(int idConversation)
        {
            List<int> result = _userConversationRepository.List().Where(x => x.ConversationId == idConversation).Select(x => x.UserId).ToList();

            return new ResponseObject(ResponseType.Success, "", result);
        }

        public ResponseObject GetConversationUsers(int idConversation)
        {
            List<UserConversation> result = _userConversationRepository.List().Where(x => x.ConversationId == idConversation).ToList();

            return new ResponseObject(ResponseType.Success, "", result);
        }

        public async Task<ResponseObject> UpdateUserConversation(UserConversation conv)
        {
            UserConversation result = await _userConversationRepository.UpdateAsync(conv);

            if(result == null)
                return new ResponseObject(ResponseType.Error);

            return new ResponseObject(ResponseType.Success, "", result);
        }
    }
}
