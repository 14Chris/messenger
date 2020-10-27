using Messenger.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public interface IUserConversationRepository
    {
        Task<UserConversation> CreateAsync(UserConversation userConversation);
        Task<UserConversation> UpdateAsync(UserConversation userConversation);
        Task<bool> DeleteAsync(UserConversation userConversation);
        IQueryable<UserConversation> List();
    }
}
