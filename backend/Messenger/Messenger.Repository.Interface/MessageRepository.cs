using Messenger.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessengerDbContext _messengerDbContext;

        public MessageRepository(MessengerDbContext messengerDbContext)
        {
            this._messengerDbContext = messengerDbContext;
        }
        public async Task<Message> CreateAsync(Message message)
        {
            try
            {
                this._messengerDbContext.Messages.Add(message);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return message;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Message message)
        {
            try
            {
                this._messengerDbContext.Messages.Remove(message);
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

        public IQueryable<Message> List()
        {
            return this._messengerDbContext.Messages.AsQueryable();
        }

        public async Task<Message> UpdateAsync(Message message)
        {
            try
            {
                this._messengerDbContext.Entry(message).State = EntityState.Modified;
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return message;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
