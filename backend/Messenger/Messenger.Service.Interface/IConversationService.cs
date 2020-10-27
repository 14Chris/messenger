using Messenger.Facade.Models;
using Messenger.Facade.Response;
using System.Threading.Tasks;

namespace Messenger.Service.Interface
{
    public interface IConversationService
    {
        public ReturnApiObject GetConversationsByUser(int id);
        public Task<ReturnApiObject> CreateConversation(int idCreator, int[] friends, string message);
        public ReturnApiObject GetConversationExistsByUsers(int[] users);
        public ReturnApiObject GetConversationById(int id, int userId);
        public ConversationListItem GetConversationListItemById(int id, int userId);
    }
}