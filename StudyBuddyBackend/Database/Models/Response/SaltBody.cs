namespace StudyBuddyBackend.Database.Models.Response
{
    public class SaltBody
    {
        public SaltBody(string salt)
        {
            Salt = salt;
        }

        public string Salt { get; }
    }
}
