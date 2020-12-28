using Messenger.Facade.Models;
using Messenger.Facade.Response;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IConversationService
    {
        public ResponseObject GetConversationsByUser(int id);

        public Task<ResponseObject> CreateConversation(int idCreator, int[] friends, MessageModel message);

        public ResponseObject GetConversationExistsByUsers(int[] users);

        public ResponseObject GetConversationById(int id, int userId);

        public ResponseObject GetConversationDetailById(int id, int userId);

        public ResponseObject GetConversationListItemById(int id, int userId);

        public Task<ResponseObject> ArchiveConversation(int conversationId, int userId);
    }
}