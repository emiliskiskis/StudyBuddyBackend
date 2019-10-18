using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Repositories;

namespace StudyBuddyBackend.Database.Controllers
{
    [Route("api/users")]
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

        // POST: api/users
        [HttpPost]
        public IActionResult Create([FromBody] User user)
        {
            _logger.LogDebug("Create user " + JsonConvert.SerializeObject(user));

            try
            {
                _userRepository.Create(user);
                return Ok(_userRepository.Read(user.Username).Value);
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.DuplicateKeyEntry)
                {
                    _logger.LogError(e.ToString());
                    return Conflict();
                }

                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return StatusCode(500, e.ToString());
            }
        }

        // GET: api/users/username
        [HttpGet("{username}")]
        public IActionResult Read(string username)
        {
            _logger.LogDebug("Get user " + username);

            var optional = _userRepository.Read(username);
            if (optional.HasValue) return Ok(optional.Value);
            return NotFound();
        }

        // PUT: api/users/username
        [HttpPut("{username}")]
        public async Task<IActionResult> Update(string username)
        {
            if (!_userRepository.Read(username).HasValue) return NotFound();
            string body = await AsyncReader.ReadAll(Request.Body);
            _logger.LogDebug("Update user " + username + ", data: " + body);

            var keys = JsonConvert.DeserializeObject<Dictionary<string, object>>(body).Keys;

            _userRepository.Update(username, keys, JsonConvert.DeserializeObject<User>(body));
            return Ok(_userRepository.Read(username));
        }

        // DELETE: api/users/username
        [HttpDelete("{username}")]
        public IActionResult Delete(string username)
        {
            _logger.LogDebug("Delete user " + username);

            try
            {
                _userRepository.Delete(username);
                return Ok();
            }
            catch (MySqlException e)
            {
                if (e.Number == (int)MySqlErrorCode.KeyDoesNotExist)
                {
                    _logger.LogError(e.ToString());
                    return BadRequest(e.ToString());
                }

                throw;
            }
        }
    }
}
