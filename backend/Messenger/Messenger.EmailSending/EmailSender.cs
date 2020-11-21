using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Messenger.EmailSending
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly SendGridEmailSenderOptions _emailSenderSettings { get; set; }
        private readonly AppSettings _appSettings { get; set; }

        public SendGridEmailSender(IOptions<SendGridEmailSenderOptions> emailSenderSettings, IOptions<AppSettings> appSettings)
        {
            this._emailSenderSettings = emailSenderSettings.Value;
            this._appSettings = appSettings.Value;
        }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string message)
        {
            await Execute(_emailSenderSettings.ApiKey, subject, message, email);
        }

        private async Task<Response> Execute(
            string apiKey,
            string subject,
            string message,
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_emailSenderSettings.SenderEmail, _emailSenderSettings.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            return await client.SendEmailAsync(msg);
        }
    }
}
