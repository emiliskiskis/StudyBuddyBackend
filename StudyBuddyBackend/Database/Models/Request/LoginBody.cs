using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Models
{
    public class LoginBody
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}
