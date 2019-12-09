using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Models.Request;
using StudyBuddyBackend.Database.Models.Response;
using StudyBuddyBackend.Identity;

namespace StudyBuddyBackend.Database.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly ILogger _logger;
        private readonly IIdentityService _identityService;

        public LoginController(IDatabaseContext databaseContext, ILogger<LoginController> logger,
            IIdentityService identityService)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _identityService = identityService;
        }

        [HttpPost("login")]
        public ActionResult<string> ApproveLogin([FromBody] LoginBody loginBody)
        {
            var user = _databaseContext.Users.Find(loginBody.Username);

            // return null if user not found
            if (user == null || user.Password != loginBody.Password)
                return BadRequest();

            var token = _identityService.Authenticate(user.Username);
            if (token != null)
            {
                return Ok(new {token});
            }

            // Else respond with Bad Request
            return BadRequest();
        }

        [HttpGet("salt/{username}")]
        public ActionResult<SaltBody> GetUserSalt(string username)
        {
            // Find the user
            var user = _databaseContext.Users.Find(username);
            // If the user doesn't exist
            if (user == null)
            {
                // Respond with Bad Request not to inform about user not existing
                return BadRequest();
            }

            // Else return the salt of user
            return new SaltBody(user.Salt);
        }
    }
}
