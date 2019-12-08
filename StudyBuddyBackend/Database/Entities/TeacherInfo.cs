using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudyBuddyBackend.Database.Entities
{
    public class TeacherInfo
    {
        [Key]
        [ForeignKey("User")]
        public string Username { get; set; }
        public User User { get; set; }

        [ReadOnly(true)]
        public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}
