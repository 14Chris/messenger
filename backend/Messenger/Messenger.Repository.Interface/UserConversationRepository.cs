using Messenger.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public class UserConversationRepository : IUserConversationRepository
    {
        private readonly MessengerDbContext _messengerDbContext;

        public UserConversationRepository(MessengerDbContext messengerDbContext)
        {
            this._messengerDbContext = messengerDbContext;
        }
        public async Task<UserConversation> CreateAsync(UserConversation userConversation)
        {
            try
            {
                this._messengerDbContext.UserConversations.Add(userConversation);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return userConversation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(UserConversation userConversation)
        {
            try
            {
                this._messengerDbContext.UserConversations.Remove(userConversation);
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

        public IQueryable<UserConversation> List()
        {
            return this._messengerDbContext.UserConversations.AsQueryable();
        }

        public async Task<UserConversation> UpdateAsync(UserConversation userConversation)
        {
            try
            {
                this._messengerDbContext.Entry(userConversation).State = EntityState.Modified;

                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return userConversation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
