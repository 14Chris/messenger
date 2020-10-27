using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Database
{
    public class Token
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("type")]
        public TokenType Type { get; set; }

        [Required]
        [JsonProperty("value")]
        public string Value { get; set; }

        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
