using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Managers;
using StudyBuddyBackend.Models;

namespace StudyBuddyBackend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger)
        {
            this.logger = logger;
        }

        // GET: api/user/username
        [HttpGet("{username}")]
        public object Get(string username)
        {
            var user = from u in UserManager.GetUsers()
                       where u.Username == username
                       select u;
            return user;
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