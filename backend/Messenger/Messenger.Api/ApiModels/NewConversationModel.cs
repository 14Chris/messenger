using Messenger.Api.ApiModels;

namespace Messenger.Api.Controllers
{
    public class NewConversationModel
    {
        public MessageModel message { get; set; }
        public int[] friends { get; set; }
    }
}