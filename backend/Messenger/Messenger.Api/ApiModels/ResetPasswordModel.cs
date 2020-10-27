namespace Messenger.Api.ApiModels
{
    public class ResetPasswordModel
    {
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}
