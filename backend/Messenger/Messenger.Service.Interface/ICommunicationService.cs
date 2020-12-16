using Messenger.Facade.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface ICommunicationService
    {
        public Task<ResponseObject> SendNewMessageNotification(int userId, dynamic requestData, CancellationToken ct = default(CancellationToken));
        public Task<ResponseObject> SendConversationCreatedNotification(int conversationId, CancellationToken ct = default(CancellationToken));
    }
}
