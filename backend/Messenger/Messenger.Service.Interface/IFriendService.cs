using Messenger.Facade.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IFriendService
    {
        public ReturnApiObject GetFriendsByUser(int userId);
        public ReturnApiObject GetFriendsRequestByUser(int userId);

        public ReturnApiObject SearchFriendsByUser(int userId, string searchTerm);
        public Task<ReturnApiObject> AddFriend(int userId, int friendId);
    }
}
