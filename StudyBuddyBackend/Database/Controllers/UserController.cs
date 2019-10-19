using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Validators;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
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
            var validationResult = _userValidator.Validate(user);

            if (!validationResult.IsValid)
            {
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
