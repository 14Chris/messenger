using Messenger.EmailSending.Models;
using System.Threading.Tasks;

namespace Messenger.EmailSending
{
    public interface IEmailSenderService
    {
        public Task<bool> SendResetPasswordEmail(ResetPasswordEmailModel model);
        public Task<bool> SendActivateAccountEmail(ActivateAccountEmailModel model);
    }
}
