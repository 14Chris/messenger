using Messenger.EmailSending.Models;
using Messenger.Facade.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Messenger.EmailSending
{
    public class EmailSenderService : IEmailSenderService
    {

        private readonly EmailSenderSettings _emailSenderSettings;
        private readonly AppSettings _appSettings;
        private readonly EmailTemplatesSettings _emailTemplatesSettings;

        public EmailSenderService(IOptions<EmailSenderSettings> emailSenderSettings, IOptions<AppSettings> appSettings, IOptions<EmailTemplatesSettings> emailTemplatesSettings)
        {
            this._emailSenderSettings = emailSenderSettings.Value;
            this._appSettings = appSettings.Value;
            this._emailTemplatesSettings = emailTemplatesSettings.Value;
        }

        /// <summary>
        /// Send reset password email request using SendGrid template
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> SendResetPasswordEmail(ResetPasswordEmailModel model)
        {
            string apiKey = _emailSenderSettings.ApiKey;

            // Create SendGrid client using api key
            SendGridClient client = new SendGridClient(apiKey);

            // Create SendGrid email message
            SendGridMessage msg = new SendGridMessage();

            // Set from informations
            msg.SetFrom(_emailSenderSettings.SenderEmail, _emailSenderSettings.SenderName);

            // Add to informations (user name and mail)
            msg.AddTo(model.userEmail, model.userName);

            // Set email SendGrid template id
            msg.SetTemplateId(_emailTemplatesSettings.ResetPasswordId);

            // Set template data
            msg.SetTemplateData(new 
            {
                userName= model.userName,
                link = model.link
            }
            );

            // Send request to send email
            var response = client.SendEmailAsync(msg).Result;

            // Return response
            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }
    }
}
