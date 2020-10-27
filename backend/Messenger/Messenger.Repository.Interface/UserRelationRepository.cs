using Messenger.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public class UserRelationRepository : IUserRelationRepository
    {
        private readonly MessengerDbContext _messengerDbContext;

        public UserRelationRepository(MessengerDbContext messengerDbContext)
        {
            this._messengerDbContext = messengerDbContext;
        }

        public async Task<UserRelation> CreateAsync(UserRelation userRelation)
        {
            try
            {
                this._messengerDbContext.UserRelations.Add(userRelation);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return userRelation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(UserRelation userRelation)
        {
            try
            {
                this._messengerDbContext.UserRelations.Remove(userRelation);
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

        public IQueryable<UserRelation> List()
        {
            return this._messengerDbContext.UserRelations.AsQueryable();
        }

        public async Task<UserRelation> UpdateAsync(UserRelation userRelation)
        {
            try
            {
                this._messengerDbContext.Entry(userRelation).State = EntityState.Modified;

                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return userRelation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
