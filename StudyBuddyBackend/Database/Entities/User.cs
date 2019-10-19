using System.Collections.Generic;
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

        public ICollection<UserChat> Chats { get; set; }
    }
}
