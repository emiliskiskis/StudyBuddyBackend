using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Request;
using StudyBuddyBackend.Database.Models.Response;

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
            // Requester is the person that initiated the request
            User requester = _databaseContext.Users.Find(userPair.Username);
            // The connectee is the person that the requester wants to connect to
            User connectee = _databaseContext.Users.Find(userPair.ConnectTo);

            // If trying to connect nobody, it's a Bad Request
            if (requester == null) return BadRequest();
            // If the person trying to connect to is not found, it's a Not Found
            if (connectee == null) return NotFound();

            // Check if a one-on-one chat already exists, since those are unique. 
            var existingChat = _databaseContext.Chats.FirstOrDefault(c => c.Users.Count == 2 && c.Users.All(userChat =>
                                                                              userChat.User.Username ==
                                                                              userPair.Username ||
                                                                              userChat.User.Username ==
                                                                              userPair.ConnectTo));
            if (existingChat != default)
            {
                // If it does, return the existing chat's group name.
                return new {existingChat.GroupName};
            }

            // If a chat does not yet exist, create a new one, add both users to it and return the group name.
            Chat chat = new Chat(Guid.NewGuid().ToString());
            chat.Users.Add(new UserInChat(requester, chat));
            chat.Users.Add(new UserInChat(connectee, chat));

            _databaseContext.Chats.Add(chat);
            _databaseContext.SaveChanges();
            return new {chat.GroupName};
        }

        [HttpGet("{groupName}")]
        public ActionResult<IEnumerable<ChatHistory>> GetAllMessages(string groupName) 
        {
            var existingChat = _databaseContext.Chats.Include(c => c.Messages).ThenInclude(m => m.User).FirstOrDefault(c => c.GroupName == groupName);       
            if (existingChat == default) 
            {
                return NotFound();
            }
            return existingChat.Messages.Select(m => new ChatHistory(m)).ToList();
        }
    }
}
