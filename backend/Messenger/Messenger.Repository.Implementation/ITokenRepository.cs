using Messenger.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Repository.Interface
{
    public interface ITokenRepository
    {
        Task<Token> CreateAsync(Token token);
        Task<Token> UpdateAsync(Token token);
        Task<bool> DeleteAsync(Token token);
        IQueryable<Token> List();
    }
}
