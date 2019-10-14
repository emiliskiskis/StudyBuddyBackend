using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Repositories;

namespace StudyBuddyBackend.Database.Controllers
{
    [Route("api/user")]
    [ApiController]
    public sealed class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserRepository _userRepository;

        public UserController(ILogger<UserController> logger, UserRepository userManager)
        {
            _logger = logger;
            _userRepository = userManager;
        }

        // POST: api/user
        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            try
            {
                _userRepository.Create(user);
                return Ok(_userRepository.Read(user.Username).Value);
            }
            catch (MySqlException e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500, e.ToString());
            }
        }

        // GET: api/user/username
        [HttpGet("{username}")]
        public IActionResult Read(string username)
        {
            var optional = _userRepository.Read(username);
            if (optional.HasValue) return Ok(optional.Value);
            return NotFound();
        }

        // PUT: api/user/username
        [HttpPut("{username}")]
        public IActionResult Update(string username, [FromBody] User newUser)
        {
            return StatusCode(500, "Unimplemented");
        }

        // DELETE: api/user/username
        [HttpDelete("{username}")]
        public IActionResult Delete(string username)
        {
            try
            {
                _userRepository.Delete(username);
                return Ok();
            }
            catch (MySqlException e)
            {
                _logger.LogError(e.ToString());
                return BadRequest(e.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500, e.ToString());
            }
        }
    }
}
