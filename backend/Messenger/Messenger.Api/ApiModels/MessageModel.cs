using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Api.ApiModels
{
    public class MessageModel
    {
        public string text { get; set; }
        public string gifId { get; set; }
        public string stickerId { get; set; }
        public string gifUrl { get; set; }
        public string stickerUrl { get; set; }
    }
}
