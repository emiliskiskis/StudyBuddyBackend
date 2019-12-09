using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class Feedback
    {
        public Feedback(Entities.Feedback feedback)
        {
            Author = new PublicUser(feedback.Author);
            Comment = feedback.Comment;
            Rating = feedback.Rating;
        }

        public PublicUser Author { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
