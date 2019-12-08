using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Subject
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        public string Name { get; set; }

        [ReadOnly(true)]
        public ICollection<TeacherSubject> Users { get; set; } = new List<TeacherSubject>();
    }
}

