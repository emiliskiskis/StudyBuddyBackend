using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Database.Models.Request;

namespace StudyBuddyBackend.Database.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public LoginController(DatabaseContext databaseContext, ILogger<LoginController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult ApproveLogin([FromBody] LoginBody loginBody)
        {
            // Find the user
            var user = _databaseContext.Users.Find(loginBody.Username);
            // If the user doesn't exist
            if (user == null)
            {
                // Respond with Bad Request not to inform about user not existing
                return BadRequest();
            }

            // If the passwords match
            if (user.Password == loginBody.Password)
            {
                // Respond with Ok
                return Ok();
            }

            // Else respond with Bad Request
            return BadRequest();
        }
    }
}
