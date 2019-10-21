namespace StudyBuddyBackend.Database.Models
{
    public class SaltBody
    {
        public SaltBody(string salt)
        {
            Salt = salt;
        }

        public string Salt { get; set; }
    }
}
