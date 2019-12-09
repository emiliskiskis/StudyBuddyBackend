using Microsoft.AspNetCore.Http;

namespace StudyBuddyBackend.Identity
{
    public interface IIdentityService
    {
        string Authenticate(string username);
        string GetUsername(HttpContext context);
    }
}
