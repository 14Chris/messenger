using Messenger.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public interface IConversationRepository
    {
        Task<Conversation> CreateAsync(Conversation conversation);
        Task<Conversation> UpdateAsync(Conversation conversation);
        Task<bool> DeleteAsync(Conversation conversation);
        IQueryable<Conversation> List();
    }
}
