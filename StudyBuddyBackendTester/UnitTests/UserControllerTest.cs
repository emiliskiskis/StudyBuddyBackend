using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Database;
using StudyBuddyBackend.Database.Controllers;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Response;
using StudyBuddyBackend.Identity;
using Xunit;
using Xunit.Abstractions;

namespace StudyBuddyBackendTester.UnitTests
{
    public class UserControllerTest
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly UserController _userController;

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

        public UserControllerTest(ITestOutputHelper testOutputHelper)
        {
            _databaseContext =
                new DatabaseContext(
                    new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("StudyBuddy").Options);
            _testOutputHelper = testOutputHelper;
            _userController = new UserController(_databaseContext, new IdentityService(),
                new LoggerFactory().CreateLogger<UserController>());
        }

        [Fact]
        public void CreateUserTest()
        {
            _databaseContext.Users.RemoveRange(_databaseContext.Users.ToList());
            var goodUser = new User
            {
                Username = "test",
                Password = "test",
                Salt = "test",
                FirstName = "test",
                LastName = "test",
                Email = "test@email.com"
            };

            var response = _userController.CreateUser(goodUser);

            Assert.StrictEqual(goodUser, response.Value);
            Assert.StrictEqual(goodUser, _databaseContext.Users.Find("test"));
        }

        [Fact]
        public void DeleteUserTest()
        {
            _databaseContext.Users.RemoveRange(_databaseContext.Users.ToList());

            var user = new User
            {
                Username = "test",
                Password = "test",
                Salt = "test",
                FirstName = "test",
                LastName = "test",
                Email = "test@email.com"
            };

            _userController.CreateUser(user);

            var response = _userController.DeleteUser(user.Username);
            _testOutputHelper.WriteLine(response.Value.ToString());
            Assert.Equal(JsonConvert.SerializeObject(new ChatlessUser(user)),
                JsonConvert.SerializeObject(response.Value));

            var response2 = _userController.DeleteUser(user.Username);
            Assert.Equal(JsonConvert.SerializeObject(new NotFoundResult()),
                JsonConvert.SerializeObject(response2.Result));
        }
    }
}