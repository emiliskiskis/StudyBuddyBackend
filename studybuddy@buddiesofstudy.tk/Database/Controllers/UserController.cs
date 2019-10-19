using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public UserController(DatabaseContext databaseContext, ILogger<UserController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return Ok(_databaseContext.Users.Find(user.Username));
        }

        [HttpGet("{username}")]
        public ActionResult<User> ReadUser(string username)
        {
            var user = _databaseContext.Users.Find(username);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet]
        public ActionResult<ICollection<User>> ReadAllUsers()
        {
            return _databaseContext.Users.ToList();
        }

        [HttpPut("{username}")]
        public ActionResult<User> UpdateUser(string username, [FromBody] User user)
        {
            if (username != user.Username)
            {
                return BadRequest();
            }

            _databaseContext.Users.Update(user);
            try
            {
                _databaseContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(username))
                {
                    return NotFound();
                }

                throw;
            }

            return _databaseContext.Users.Find(username);

        }

        [HttpPatch("{username}")]
        public ActionResult<User> PatchUser(string username, [FromBody] JsonPatchDocument<User> patch)
        {
            var user = _databaseContext.Users.Find(username);
            if (user == null)
            {
                return NotFound();
            }
            
            patch.ApplyTo(user);

            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();

            return _databaseContext.Users.Find(username);
        }

        [HttpDelete("{username}")]
        public ActionResult<User> DeleteUser(string username)
        {
            var user = _databaseContext.Users.Find(username);
            if (user == null)
            {
                return NotFound();
            }

            _databaseContext.Users.Remove(user);
            _databaseContext.SaveChanges();

            return user;
        }

        private bool UserExists(string username)
        {
            return _databaseContext.Users.Any(u => u.Username == username);
        }
    }
}
