using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Facade.Models
{
    public class ConversationListItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("last_message")]
        public string LastMessage { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_message_date")]
        public DateTime LastMessageDate { get; set; }

        [JsonProperty("last_message_sender")]
        public UserBasicModel LastMessageSender { get; set; }

        [JsonProperty("friends_ids")]
        public List<int> FriendsIds { get; set; }
    }
}
