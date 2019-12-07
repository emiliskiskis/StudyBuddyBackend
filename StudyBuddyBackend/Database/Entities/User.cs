using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class User
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255, MinimumLength = 1)]
        [Required]
        [Key]
        public string Username { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Required]
        public string Password { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Required]
        public string Salt { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Required]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Required]
        public string LastName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        // Initializing lists because empty sets in database create null values
        [ReadOnly(true)]
        public ICollection<UserInChat> Chats { get; set; } = new List<UserInChat>();
        [ReadOnly(true)]
        public ICollection<UserSubject> Subjects { get; set; } = new List<UserSubject>();
    }
}
