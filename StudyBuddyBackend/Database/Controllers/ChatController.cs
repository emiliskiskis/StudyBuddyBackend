using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Request;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public ChatController(DatabaseContext databaseContext, ILogger<UserController> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }
        
        [HttpPost]
        public ActionResult<object> ConnectToUser(UserPair userPair)
        {
            Chat chat = new Chat(new Guid().ToString());
            User requester = _databaseContext.Users.Find(userPair.Username);
            User connectee = _databaseContext.Users.Find(userPair.ConnectTo);
            
            if (requester == null) return BadRequest();
            if (connectee == null) return NotFound();
            
            chat.Users.Add(new UserChat(requester, chat));
            chat.Users.Add(new UserChat(connectee, chat));
            
            _databaseContext.Chats.Add(chat);
            return new {chat.GroupName};
        }
    }
}
