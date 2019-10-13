using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Managers;

namespace StudyBuddyBackend.Database.Controllers
{
    [Route("api/user")]
    [ApiController]
    public sealed class UserController : ControllerBase
    {
        private readonly UserRepository userRepository;

        public UserController(UserRepository userManager)
        {
            this.userRepository = userManager;
        }

        // GET: api/user/username
        [HttpGet("{username}")]
        public IActionResult Get(string username)
        {
            var optional = userRepository.Get(username);
            if (optional.HasValue) return Ok(optional.Value);
            else return NotFound();
        }

        // POST: api/user
        [HttpPost]
        public void Post([FromBody] string value)
        {
            User x = JsonConvert.DeserializeObject<User>(value);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            User x = JsonConvert.DeserializeObject<User>(value);
        }

        // DELETE: api/user/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
