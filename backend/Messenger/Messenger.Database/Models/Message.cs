using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Messenger.Database
{
    public class Message
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("gif_id")]
        public string GifId { get; set; }

        [JsonProperty("sticker_id")]
        public string StickerId { get; set; }

        [Required]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [Required]
        [JsonProperty("sender_id")]
        public int SenderId { get; set; }

        [Required]
        [JsonProperty("conversation_id")]
        public int ConversationId { get; set; }

        public User Sender { get; set; }

        public Conversation Conversation { get; set; }
    }
}
