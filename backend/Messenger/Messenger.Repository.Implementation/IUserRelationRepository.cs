using Messenger.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Repository.Implementation
{
    public interface IUserRelationRepository
    {
        Task<UserRelation> CreateAsync(UserRelation userRelation);
        Task<UserRelation> UpdateAsync(UserRelation userRelation);
        Task<bool> DeleteAsync(UserRelation userRelation);
        IQueryable<UserRelation> List();
    }
}
