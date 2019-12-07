using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class User
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Key]
        public string Username { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Password { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Salt { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string FirstName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string LastName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        public string Email { get; set; }

        // Initializing lists because empty sets in database create null values
        [ReadOnly(true)]
        public ICollection<UserInChat> Chats { get; set; } = new List<UserInChat>();
        [ReadOnly(true)]
        public ICollection<UserSubject> Subjects { get; set; } = new List<UserSubject>();
    }
}
