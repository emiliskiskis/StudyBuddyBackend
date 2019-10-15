using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Attributes;

namespace StudyBuddyBackend.Database.Entities
{
    [Serializable]
    public class User
    {
        [PrimaryKey]
        public string Username { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }

        [JsonProperty("first_name")]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        
        [JsonProperty("last_name")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
