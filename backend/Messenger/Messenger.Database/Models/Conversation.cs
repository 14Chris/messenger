using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Messenger.Database
{
    public class Conversation
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("creator_id")]
        public int CreatorId { get; set; }

        public User Creator { get; set; }

        public List<UserConversation> Conversations { get; set; }

        public List<Message> Messages { get; set; }
    }
}
