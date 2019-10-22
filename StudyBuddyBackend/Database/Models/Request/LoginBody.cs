using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Models.Request
{
    public class LoginBody
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}
