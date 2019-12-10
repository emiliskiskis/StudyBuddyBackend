using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Database;
using StudyBuddyBackend.Database.Controllers;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Request;
using StudyBuddyBackend.Database.Models.Response;
using StudyBuddyBackend.Identity;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace StudyBuddyBackendTester.UnitTests
{
    public class FeedbackControllerTest
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly FeedbackController _feedbackController;

        private class IdentityService : IIdentityService
        {
            public string Authenticate(string username)
            {
                throw new System.NotImplementedException();
            }

            public string GetUsername(HttpContext context)
            {
                return "test";
            }
        }

        public FeedbackControllerTest(ITestOutputHelper testOutputHelper)
        {
            _databaseContext =
                new DatabaseContext(
                    new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("StudyBuddy").Options);
            _testOutputHelper = testOutputHelper;
            _feedbackController = new FeedbackController(_databaseContext, new IdentityService());
        }

        [Fact]
        public void CreateFeedbackTest()
        {
            _databaseContext.Feedback.RemoveRange(_databaseContext.Feedback.ToList());
            var goodFeedback = new StudyBuddyBackend.Database.Entities.Feedback
            {
                AuthorUsername = "titas",
                RevieweeUsername = "emilis",
                Comment = "test",
                Rating = 5
            };

            _feedbackController.AddFeedback(goodFeedback);

            Assert.StrictEqual(goodFeedback, _databaseContext.Feedback.Find("titas", "emilis"));
        }

        [Fact]
        public void DeleteFeedbackTest()
        {
            _databaseContext.Feedback.RemoveRange(_databaseContext.Feedback.ToList());
            var goodFeedback = new StudyBuddyBackend.Database.Entities.Feedback
            {
                AuthorUsername = "titas",
                RevieweeUsername = "emilis",
                Comment = "test",
                Rating = 5
            };

            _feedbackController.AddFeedback(goodFeedback);

            var feedback_res = _feedbackController.DeleteFeedback(new FeedbackPair(goodFeedback.AuthorUsername, goodFeedback.RevieweeUsername));
            //_testOutputHelper.WriteLine(feedback_res.Value.ToString());
            Assert.Equal(JsonConvert.SerializeObject(new StudyBuddyBackend.Database.Models.Response.Feedback(goodFeedback)),
               JsonConvert.SerializeObject(feedback_res.Value));
        }
    }
}
