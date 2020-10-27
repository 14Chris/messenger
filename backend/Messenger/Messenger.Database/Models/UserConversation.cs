using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Database
{
    public class UserConversation
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("visibility")]
        public ConversationVisibility Visibility { get; set; }

        [Required]
        public int ConversationId { get; set; }

        [Required]
        public int UserId { get; set; }

        public Conversation Conversation { get; set; }
        
        public User User { get; set; }

    }
}
