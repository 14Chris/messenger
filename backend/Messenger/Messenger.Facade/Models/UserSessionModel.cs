using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Facade.Models
{
    public class UserSessionModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }
    }
}
