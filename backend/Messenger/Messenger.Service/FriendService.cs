using Messenger.Database;
using Messenger.Facade.Response;
using Messenger.Facade.Settings;
using Messenger.Service.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messenger.Facade.Models;

namespace Messenger.Service.Implementation
{
    public class FriendService : BaseService, IFriendService
    {
        public FriendService(IServiceProvider serviceProvider, IOptions<JwtSettings> jwtSettings) : base(
            serviceProvider, jwtSettings)
        {
        }

        /// <summary>
        /// Get user friends
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ReturnApiObject GetFriendsByUser(int userId)
        {
            List<UserBasicModel> friends = _userRelationRepository.List()
                .Where(x => (x.FriendId == userId) && x.State == UserRelationState.Accepted).Select(x => x.User)
                .Union(_userRelationRepository.List()
                    .Where(x => (x.UserId == userId) && x.State == UserRelationState.Accepted).Select(x => x.Friend))
                .Select(x => new UserBasicModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                })
                .ToList();

            return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success, "", friends);
        }

        /// <summary>
        /// Search friends for a user by a search term
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ReturnApiObject SearchFriendsByUser(int userId, string searchTerm)
        {
            List<UserBasicModel> friends = _userRelationRepository.List().Where(x =>
                    (x.FriendId == userId) && x.State == UserRelationState.Accepted &&
                    (x.User.LastName.StartsWith(searchTerm) || x.User.FirstName.StartsWith(searchTerm)))
                .Select(x => x.User)
                .Union(_userRelationRepository.List().Where(x => (x.UserId == userId) &&
                                                                 x.State == UserRelationState.Accepted &&
                                                                 (x.Friend.LastName.StartsWith(searchTerm)
                                                                  || x.Friend.FirstName.StartsWith(searchTerm)))
                    .Select(x => x.Friend))
                .Select(x=>new UserBasicModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                }).ToList();

            return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success, "", friends);
        }

        /// <summary>
        /// Add a friend request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> AddFriend(int userId, string friendEmail)
        {

            User friend = _userRepository.List().Where(x => x.Email == friendEmail).SingleOrDefault();

            if(friend == null)
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error, "FRIEND_DOESNT_EXIST", null);
            }

            UserRelation relation = RetrieveUserRelation(userId, friend.Id);

            if (relation == null)
            {
                relation = new UserRelation();
                relation.UserId = userId;
                relation.FriendId = friend.Id;
                relation.State = UserRelationState.Requested;

                UserRelation result = await _userRelationRepository.CreateAsync(relation);

                if (result == null)
                {
                    return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error);
                }

                return new ReturnApiObject(System.Net.HttpStatusCode.Created, ResponseType.Success, "", result);
            }
            else if (relation.State == UserRelationState.Deleted)
            {
                relation.State = UserRelationState.Requested;


                UserRelation result = await _userRelationRepository.UpdateAsync(relation);

                if (result == null)
                {
                    return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error);
                }

                return new ReturnApiObject(System.Net.HttpStatusCode.Created, ResponseType.Success, "", result);
            }
            else
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error);
            }
        }

        /// <summary>
        /// Delete a friend
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public ReturnApiObject RemoveFriend(int userId, int friendId)
        {
            return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success);
        }

        private UserRelation RetrieveUserRelation(int userId, int friendId)
        {
            UserRelation relation = _userRelationRepository.List().Where(x =>
                    (x.FriendId == userId && x.UserId == friendId) || (x.UserId == userId && x.FriendId == friendId))
                .SingleOrDefault();

            return relation;
        }

        /// <summary>
        /// Get all friend requests
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ReturnApiObject GetFriendsRequestByUser(int userId)
        {
            List<UserBasicModel> friends = _userRelationRepository.List()
                .Where(x => (x.FriendId == userId) && x.State == UserRelationState.Requested).Select(x => x.User)
                .Select(x => new UserBasicModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                })
                .ToList();

            return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success, "", friends);
        }

        /// <summary>
        /// Accept a friend request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> AcceptFriendRequest(int userId, int friendId)
        {
            UserRelation relation = RetrieveUserRelation(userId, friendId);

            if(relation == null)
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error, "NO_REQUEST", null);
            }

            if(relation.State == UserRelationState.Requested)
            {

                relation.State = UserRelationState.Accepted;

                UserRelation result = await _userRelationRepository.UpdateAsync(relation);

                return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success, "", null);
            }
            else
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error, "NOT_A_REQUEST", null);
            }
        }

        /// <summary>
        /// Delete a friend request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task<ReturnApiObject> DeleteFriendRequest(int userId, int friendId)
        {
            UserRelation relation = RetrieveUserRelation(userId, friendId);

            if (relation == null)
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error, "NO_REQUEST", null);
            }

            if (relation.State == UserRelationState.Requested)
            {

                relation.State = UserRelationState.Deleted;

                UserRelation result = await _userRelationRepository.UpdateAsync(relation);

                return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success, "", null);
            }
            else
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest, ResponseType.Error, "NOT_A_REQUEST", null);
            }
        }

        public async Task<ReturnApiObject> DeleteFriend(int userId, int friendId)
        {
            UserRelation relation = RetrieveUserRelation(userId, friendId);

            if (relation != null)
            {

                relation.State = UserRelationState.Deleted;

                UserRelation result = await _userRelationRepository.UpdateAsync(relation);

                return new ReturnApiObject(System.Net.HttpStatusCode.OK, ResponseType.Success, "", null);
            }
            else
            {
                return new ReturnApiObject(System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}