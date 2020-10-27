using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Database
{
    public class UserRelation
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("state")]
        public UserRelationState State { get; set; }

        [Required]
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [Required]
        [JsonProperty("friend_id")]
        public int FriendId { get; set; }

        public User User { get; set; }

        public User Friend { get; set; }
    }
}
