using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Response;
using StudyBuddyBackend.Database.Validators;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;
        private readonly UserValidator _userValidator;

        public UserController(DatabaseContext databaseContext, ILogger<UserController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _userValidator = new UserValidator();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            // Add the user and return the created user
            _databaseContext.Users.Add(user);
            _databaseContext.SaveChanges();
            return _databaseContext.Users.Find(user.Username);
        }

        [Authorize]
        [HttpGet("{username}")]
        public ActionResult<PasswordlessUser> ReadUser(string username)
        {
            var nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (nameClaim != username)
            {
                return Unauthorized();
            }

            var user = _databaseContext.Users.Find(username);
            if (user == null)
            {
                return NotFound();
            }

            return new PasswordlessUser(user);
        }

        [HttpGet]
        public ActionResult<IEnumerable<PublicUser>> ReadAllUsers()
        {
            // Return all users' usernames, first and last names
            return _databaseContext.Users.Select(user => new PublicUser(user)).ToList();
        }

        [HttpPut("{username}")]
        public ActionResult<object> UpdateUser(string username, [FromBody] User user)
        {
            var nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (nameClaim != username)
            {
                return Unauthorized();
            }

            // If the username in body doesn't match the username in the URI
            if (username != user.Username)
            {
                // Respond with Bad Request
                return BadRequest();
            }

            // Update and save the changes
            _databaseContext.Users.Update(user);
            try
            {
                _databaseContext.SaveChanges();
            }
            catch (DbUpdateException) // If failed
            {
                // If the user doesn't exist
                if (!UserExists(username))
                {
                    // Respond with Not Found
                    return NotFound();
                }

                // Else throw
                throw;
            }

            // Return partial info of the updated user
            var updatedUser = _databaseContext.Users.Find(username);
            return new ChatlessUser(updatedUser);
        }

        [HttpPatch("{username}")]
        public ActionResult<ChatlessUser> PatchUser(string username, [FromBody] JsonPatchDocument<User> patch)
        {
            var nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (nameClaim != username)
            {
                return Unauthorized();
            }

            // Find existing user
            var user = _databaseContext.Users.Find(username);
            // If doesn't exist
            if (user == null)
            {
                // Respond with Not Found
                return NotFound();
            }

            // Apply the patch and validate the result
            patch.ApplyTo(user);
            var validationResult = _userValidator.Validate(user);

            // If invalid
            if (!validationResult.IsValid)
            {
                // Group errors by field name into lists, then respond with ValidationProblemDetails
                var groupedErrors = new Dictionary<string, ICollection<string>>();

                foreach (var error in validationResult.Errors)
                {
                    var field = error.PropertyName;
                    if (!groupedErrors.ContainsKey(field))
                    {
                        groupedErrors.Add(field, new List<string>());
                    }

                    groupedErrors[field].Add(error.ErrorMessage);
                }

                var errorArrays = new Dictionary<string, string[]>();

                foreach (var (fieldName, errorList) in groupedErrors)
                {
                    errorArrays.Add(fieldName, errorList.ToArray());
                }

                return BadRequest(new ValidationProblemDetails(errorArrays));
            }

            // Update and save the user
            _databaseContext.Users.Update(user);
            _databaseContext.SaveChanges();

            // Return partial info of the updated user
            var updatedUser = _databaseContext.Users.Find(username);
            return new ChatlessUser(updatedUser);
        }

        [HttpDelete("{username}")]
        public ActionResult<ChatlessUser> DeleteUser(string username)
        {
            var nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (nameClaim != username)
            {
                return Unauthorized();
            }

            // Find the user
            var user = _databaseContext.Users.Find(username);
            // If doesn't exist
            if (user == null)
            {
                // Respond with Not Found
                return NotFound();
            }

            // Remove and save changes
            _databaseContext.Users.Remove(user);
            _databaseContext.SaveChanges();

            // Return partial info of deleted user
            return new ChatlessUser(user);
        }

        private bool UserExists(string username)
        {
            // Does any user with specified username exist?
            return _databaseContext.Users.Any(u => u.Username == username);
        }
    }
}
