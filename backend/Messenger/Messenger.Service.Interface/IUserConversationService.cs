using Messenger.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IUserConversationService
    {
        public Task<UserConversation> UpdateUserConversation(UserConversation user);
        public List<int> GetConversationIdUsers(int idConversation);
        public List<UserConversation> GetConversationUsers(int idConversation);
    }
}
