using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Response;
using StudyBuddyBackend.Database.Validators;
using ProfilePicture = StudyBuddyBackend.Database.Models.Request.ProfilePicture;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ILogger _logger;
        private readonly UserValidator _userValidator;

        public UserController(IDatabaseContext databaseContext, ILogger<UserController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _userValidator = new UserValidator();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (UserExists(user.Username)) return Conflict();

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

        [HttpPost("{username}/picture")]
        public ActionResult<ProfilePicture> AddProfilePicture(string username, ProfilePicture profilePicture)
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

            var bytes = Convert.FromBase64String(
                profilePicture.Data.Replace(
                    Regex.Match(profilePicture.Data, @".*base64,").Value, ""));
            Image image;
            using (var stream = new MemoryStream(bytes))
            {
                image = Image.FromStream(stream);
            }

            var srcRect = new Rectangle(0, 0, image.Width, image.Height);
            var destRect = new Rectangle(0, 0, 512, 512);
            var bitmap = new Bitmap(destRect.Width, destRect.Height);

            if (image.Width != image.Height)
            {
                if (image.Width > 512 || image.Height > 512)
                {
                    if (image.Width > image.Height)
                    {
                        srcRect = new Rectangle((image.Width - image.Height) / 2, 0,
                            image.Height, image.Height);
                    }
                    else
                    {
                        srcRect = new Rectangle(image.Width, (image.Height - image.Width) / 2,
                            image.Width, image.Width);
                    }
                }
                else if (image.Width < 512 && image.Height < 512)
                {
                    if (image.Width > image.Height)
                    {
                        double ratio = 512 / (double)image.Width;
                        destRect = new Rectangle(0, (int)((512 - (double)image.Height) / 2 / ratio),
                            512, (int)(image.Height * ratio));
                    }
                    else
                    {
                        double ratio = 512 / (double)image.Height;
                        destRect = new Rectangle((int)((512 - (double)image.Width) / 2 / ratio), 0,
                            (int)(image.Width * ratio), 512);
                    }
                }
            }

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            }

            string processedProfilePicture;
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                processedProfilePicture = Convert.ToBase64String(stream.ToArray());
            }

            var existingProfilePicture = _databaseContext.ProfilePictures.Find(username);
            if (existingProfilePicture != null)
            {
                existingProfilePicture.Data =
                    "data:image/png;base64," + processedProfilePicture;
            }
            else
            {
                _databaseContext.ProfilePictures.Add(new Entities.ProfilePicture(username,
                    "data:image/png;base64," + processedProfilePicture));
            }

            _databaseContext.SaveChanges();

            return new ProfilePicture(_databaseContext.ProfilePictures.Find(username));
        }

        [HttpGet("{username}/picture")]
        public ActionResult<ProfilePicture> GetProfilePicture(string username)
        {
            // Find existing user
            var profilePicture = _databaseContext.ProfilePictures.Find(username);
            // If doesn't exist
            if (profilePicture == null)
            {
                // Respond with Not Found
                return NotFound();
            }

            return new ProfilePicture(profilePicture);
        }

        [HttpDelete("{username}/picture")]
        public IActionResult DeleteProfilePicture(string username)
        {
            var nameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (nameClaim != username)
            {
                return Unauthorized();
            }

            // Find profile picture
            var profilePicture = _databaseContext.ProfilePictures.Find(username);

            if (profilePicture == null)
            {
                // Respond with Not Found
                return NotFound();
            }

            _databaseContext.ProfilePictures.Remove(profilePicture);
            _databaseContext.SaveChanges();

            return Ok();
        }

        private bool UserExists(string username)
        {
            // Does any user with specified username exist?
            return _databaseContext.Users.Any(u => u.Username == username);
        }
    }
}
