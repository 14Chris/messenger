using Messenger.Facade.Response;
using Messenger.Database;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IUserService
    {
        public Task<ReturnApiObject> CreateUser(User user);
      
        public ReturnApiObject GetUser(int id);

        public Task<ReturnApiObject> UpdateUser(User user);

        public ReturnApiObject EmailAlreadyExists(string email);

        public ReturnApiObject Login(string email, string password);

        public Task<ReturnApiObject> UpdateUserPassword(int userId, string oldPassword, string newPassword);

        public Task<ReturnApiObject> ForgotPassword(string email);

        public Task<ReturnApiObject> ValidateTokenPasswordReset(string token);

        public Task<ReturnApiObject> ResetPassword(string token, string newPassword);
    }
}
