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
            var user = _databaseContext.Users.Find(loginBody.Username);
            if (user == null)
            {
                return BadRequest();
            }

            if (user.Password == loginBody.Password)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
