using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Feedback
    {
        [Required]
        [ForeignKey("Author")]
        public string AuthorUsername { get; set; }

        [Required]
        [ForeignKey("Reviewee")]
        public string RevieweeUsername { get; set; }

        [ReadOnly(true)]
        public User Author { get; set; }

        [ReadOnly(true)]
        public User Reviewee { get; set; }

        [Required]
        public string Comment { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
