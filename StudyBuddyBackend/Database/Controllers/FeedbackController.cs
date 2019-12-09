using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Request;
using StudyBuddyBackend.Identity;

namespace StudyBuddyBackend.Database.Controllers
{
    [Route("api/feedback")]
    public class FeedbackController : Controller
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IIdentityService _identityService;

        public FeedbackController(IDatabaseContext databaseContext, IIdentityService identityService)
        {
            _databaseContext = databaseContext;
        }

        [HttpPost]
        public ActionResult<Feedback> AddFeedback([FromBody] Feedback feedback)
        {
            if (feedback.AuthorUsername != _identityService.GetUsername(HttpContext)) return Unauthorized();

            if (FeedbackExists(feedback.AuthorUsername, feedback.RevieweeUsername)) return Conflict();

            _databaseContext.Feedback.Add(feedback);
            _databaseContext.SaveChanges();
            return Ok();
        }

        private bool FeedbackExists(string authorUsername, string revieweeUsername)
        {
            return _databaseContext.Feedback.Any(f => f.AuthorUsername == authorUsername && f.RevieweeUsername == revieweeUsername);
        }

        [HttpGet("{username}")]
        public ActionResult<IEnumerable<Models.Response.Feedback>> GetFeedback(string username)
        {
            return _databaseContext.Feedback.Include(f => f.Author).Where(f => f.RevieweeUsername == username).Select(f => new Models.Response.Feedback(f)).ToList();
        }

        [HttpDelete]
        public ActionResult<IEnumerable<Models.Response.Feedback>> DeleteFeedback([FromBody] FeedbackPair pair)
        {
            var feedback = _databaseContext.Feedback.Find(pair.Author, pair.Reviewee);

            if (feedback == null)
            {
                return NotFound();
            }

            _databaseContext.Feedback.Remove(feedback);
            _databaseContext.SaveChanges();
            return Ok();
        }
    }
}
