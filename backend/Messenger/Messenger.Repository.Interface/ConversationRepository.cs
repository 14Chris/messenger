using Messenger.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public class ConversationRepository : IConversationRepository
    {

        private readonly MessengerDbContext _messengerDbContext;

        public ConversationRepository(MessengerDbContext messengerDbContext)
        {
            this._messengerDbContext = messengerDbContext;
        }

        public async Task<Conversation> CreateAsync(Conversation conversation)
        {
            try
            {
                this._messengerDbContext.Conversations.Add(conversation);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return conversation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Conversation conversation)
        {
            try
            {
                this._messengerDbContext.Conversations.Remove(conversation);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable<Conversation> List()
        {
            return this._messengerDbContext.Conversations.AsQueryable();
        }

        public async Task<Conversation> UpdateAsync(Conversation conversation)
        {
            try
            {
                this._messengerDbContext.Entry(conversation).State = EntityState.Modified;

                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return conversation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
