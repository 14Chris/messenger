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

        [JsonProperty("gif_id")]
        public string GifId { get; set; }

        [JsonProperty("sticker_id")]
        public string StickerId { get; set; }

        [JsonProperty("gif_url")]
        public string GifUrl { get; set; }

        [JsonProperty("sticker_url")]
        public string StickerUrl { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
