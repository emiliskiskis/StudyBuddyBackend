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
        [Required] public string Author { get; set; }
        [Required] public string Reviewee { get; set; }
    }
}
