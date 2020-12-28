using Messenger.Database;
using Messenger.Facade.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IMessageService
    {
        public Task<ResponseObject> CreateMessage(int userId, Message message);

        public ResponseObject LoadMoreMessagesFromConversation(int userId, int conversationId, int lastMessageId);

    }
}
