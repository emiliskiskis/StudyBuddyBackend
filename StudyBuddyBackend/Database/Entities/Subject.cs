using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Subject
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        public ICollection<UserSubject> Users { get; set; } = new List<UserSubject>();
    }
}

