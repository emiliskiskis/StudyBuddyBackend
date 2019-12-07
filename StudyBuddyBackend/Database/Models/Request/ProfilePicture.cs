namespace StudyBuddyBackend.Database.Models.Request
{
    public class ProfilePicture
    {
        public string Data { get; set; }

        public ProfilePicture()
        {
            
        }

        public ProfilePicture(Entities.ProfilePicture profilePicture)
        {
            Data = profilePicture.Data;
        }
    }
}
