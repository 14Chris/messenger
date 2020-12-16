using Messenger.Facade.Response;
using Messenger.Database;
using System.Threading.Tasks;
using Messenger.Facade.Models;
using Messenger.EmailSending.Models;

namespace Messenger.Service.Interface
{
    public interface IUserService
    {
        public Task<ResponseObject> CreateUser(User user);
      
        public ResponseObject GetUser(int id);

        public ResponseObject GetUserSession(int id);

        public Task<ResponseObject> UpdateUserInformations(UserBasicModel user);

        public ResponseObject EmailAlreadyExists(string email);

        public ResponseObject Login(string email, string password);

        public Task<ResponseObject> UpdateUserPassword(int userId, string oldPassword, string newPassword);

        public Task<ResponseObject> ForgotPassword(string email);

        public Task<ResponseObject> ValidateTokenPasswordReset(string token);

        public Task<ResponseObject> ResetPassword(string token, string newPassword);

        public ResponseObject GetUserProfile(int id);

        public ResponseObject GetUserProfilePicture(int id);

        public Task<ResponseObject> ChangeUserProfilePicture(int id, string pictureBase64);

        public Task<ResponseObject> DeleteUserProfilePicture(int id);
        public Task<ResponseObject> ActivateAccount(string token);
        public Task<ResponseObject> ResendAccountActivationEmail(string email);
    }
}
