using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class TeacherSubject
    {
        [ForeignKey("Subject")]
        public string SubjectName { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }
        public Subject Subject { get; set; }
        public TeacherInfo User { get; set; }

        public TeacherSubject()
        {

        }

        public TeacherSubject(TeacherInfo user, Subject subject)
        {
            User = user;
            Subject = subject;
        }
    }
}
