using Messenger.EmailSending.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.EmailSending
{
    public interface IEmailSenderService
    {
        public Task<bool> SendResetPasswordEmail(ResetPasswordEmailModel model);
    }
}
