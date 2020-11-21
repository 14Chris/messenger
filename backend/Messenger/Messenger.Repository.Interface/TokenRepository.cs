using Messenger.Database;
using Messenger.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly MessengerDbContext _messengerDbContext;

        public TokenRepository(MessengerDbContext messengerDbContext)
        {
            this._messengerDbContext = messengerDbContext;
        }

        public async Task<Token> CreateAsync(Token token)
        {
            try
            {
                this._messengerDbContext.Tokens.Add(token);
                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Token token)
        {
            try
            {
                this._messengerDbContext.Tokens.Remove(token);
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

        public IQueryable<Token> List()
        {
            return this._messengerDbContext.Tokens.AsQueryable();
        }

        public async Task<Token> UpdateAsync(Token token)
        {
            try
            {
                this._messengerDbContext.Entry(token).State = EntityState.Modified;

                int isSaved = await this._messengerDbContext.SaveChangesAsync();

                if (isSaved == 0)
                {
                    return null;
                }

                return token;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
