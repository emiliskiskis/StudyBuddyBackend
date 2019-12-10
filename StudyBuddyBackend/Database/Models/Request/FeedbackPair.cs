using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Request
{
    public class FeedbackPair
    {
        public FeedbackPair(string author, string reviewee)
        {
            Author = author;
            Reviewee = reviewee;
        }

        [Required] public string Author { get; set; }
        [Required] public string Reviewee { get; set; }
    }
}
