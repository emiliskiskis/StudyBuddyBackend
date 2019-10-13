using System;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Attributes;

namespace StudyBuddyBackend.Database.Entities
{
    [Serializable]
    public class User
    {
        [Id]
        public string Username { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
