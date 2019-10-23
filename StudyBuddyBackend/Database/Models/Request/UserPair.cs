using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Models.Request
{
    public class UserPair
    {
        [Required] public string Username { get; set; }
        [Required] public string ConnectTo { get; set; }
    }
}
