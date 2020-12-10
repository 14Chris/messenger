using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Facade.Models
{
    public class ConversationDetailModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("friends")]
        public List<UserBasicModel> Friends { get; set; }

    }
}
