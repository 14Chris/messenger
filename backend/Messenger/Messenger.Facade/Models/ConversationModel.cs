using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Messenger.Facade.Models
{
    public class ConversationModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("messages")]
        public List<MessageModel> Messages { get; set; }
    }
}
