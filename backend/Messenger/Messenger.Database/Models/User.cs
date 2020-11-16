using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Database
{
    public class User
    {
        [Key]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("profile_picture")]
        public byte[] ProfilePicture { get; set; }

        public List<Message> MessagesSended { get; set; }

        public List<UserConversation> Conversations { get; set; }
        
        public List<Conversation> ConversationsCreated { get; set; }

        public List<UserRelation> FriendsAdded { get; set; }

        public List<UserRelation> Friends { get; set; }

        public List<Token> Tokens { get; set; }

    }
}
