using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace StudyBuddyBackend.Database.Entities
{
    public class User
    {
        [Column(TypeName = "VARCHAR")]
        [StringLength(255)]
        [Key]
        [Required]
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

        public ICollection<UserChat> Chats { get; set; }
    }
}
