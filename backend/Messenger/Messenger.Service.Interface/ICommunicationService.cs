using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface ICommunicationService
    {
        public Task SendNewMessageNotification(int userId, dynamic requestData, CancellationToken ct = default(CancellationToken));
        public Task SendConversationCreatedNotification(int conversationId, CancellationToken ct = default(CancellationToken));
    }
}
