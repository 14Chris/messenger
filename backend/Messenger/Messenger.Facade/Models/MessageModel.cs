using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Facade.Models
{
    public class MessageModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("sender_id")]
        public int SenderId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
