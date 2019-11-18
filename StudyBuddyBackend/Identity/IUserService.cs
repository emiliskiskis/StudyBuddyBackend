namespace StudyBuddyBackend.Identity
{
    public interface IUserService
    {
        string Authenticate(string username);
    }
}
