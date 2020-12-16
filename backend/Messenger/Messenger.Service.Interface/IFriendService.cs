using Messenger.Facade.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IFriendService
    {
        public ResponseObject GetFriendsByUser(int userId);
        public ResponseObject GetFriendsRequestByUser(int userId);

        public ResponseObject SearchFriendsByUser(int userId, string searchTerm);
        public Task<ResponseObject> AddFriend(int userId, string friendEmail);
        public Task<ResponseObject> AcceptFriendRequest(int userId, int friendId);
        public Task<ResponseObject> DeleteFriendRequest(int userId, int friendId);
        public Task<ResponseObject> DeleteFriend(int userId, int friendId);
    }
}
