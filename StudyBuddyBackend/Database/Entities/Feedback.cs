using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Feedback
    {
        [ForeignKey("Author")]
        public string AuthorUsername { get; set; }
        [ForeignKey("Reviewer")]
        public string ReviewerUsername { get; set; }
        public User Author { get; set; }
        public User Reviewer { get; set; }
        public string Comment { get; set; }
    }
}
