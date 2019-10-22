using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Hubs;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;
        private readonly ChatHub _chatHub;

        public ChatController(DatabaseContext databaseContext, ILogger<UserController> logger, ChatHub chatHub)
        {
            _databaseContext = databaseContext;
            _logger = logger;
            _chatHub = chatHub;
        }
        
        [HttpPost]
        public async Task<ActionResult<String>> AddUserToGroup([FromBody] String username)
        {
            await (_chatHub.Connect(username, null));
            String groupName = _chatHub.GroupName;
            return Ok(groupName);
        }
    }
}