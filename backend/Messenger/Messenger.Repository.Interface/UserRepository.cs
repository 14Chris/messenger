using Messenger.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly MessengerDbContext _messengerDbContext;

        public UserRepository(MessengerDbContext messengerDbContext)
        {
            this._messengerDbContext = messengerDbContext;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> CreateAsync(User user)
        {
            try
            {
                this._messengerDbContext.Users.Add(user);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(User user)
        {
            try
            {
                this._messengerDbContext.Users.Remove(user);
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

        /// <summary>
        /// Get a user by an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetById(int id)
        {
            User user = await this._messengerDbContext.Users.FindAsync(id);

            return user;
        }

        public IQueryable<User> List()
        {
            return this._messengerDbContext.Users.AsQueryable();
        }

        public async Task<User> UpdateAsync(User user)
        {
            try
            {
                this._messengerDbContext.Entry(user).State = EntityState.Modified;

                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
