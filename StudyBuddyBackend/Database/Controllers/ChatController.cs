using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database.Entities;
using StudyBuddyBackend.Database.Models.Request;
using StudyBuddyBackend.Database.Models.Response;
using StudyBuddyBackend.Identity;
using Chat = StudyBuddyBackend.Database.Models.Response.Chat;

namespace StudyBuddyBackend.Database.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IIdentityService _identityService;
        private readonly ILogger _logger;

        public ChatController(IDatabaseContext databaseContext, IIdentityService identityService, ILogger<UserController> logger)
        {
            _databaseContext = databaseContext;
            _identityService = identityService;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<Chat> ConnectToUser(UserPair userPair)
        {
            var nameClaim = _identityService.GetUsername(HttpContext);

            if (nameClaim != userPair.Username)
            {
                return Unauthorized();
            }

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
                // If it does, return the existing chat's id.
                return default; //new Chat(existingChat, GetAllUsers(existingChat.Id).Value);
            }

            // If a chat does not yet exist, create a new one, add both users to it and return the chat's id.
            Entities.Chat chat = new Entities.Chat(Guid.NewGuid().ToString());
            chat.Users.Add(new UserInChat(requester, chat));
            chat.Users.Add(new UserInChat(connectee, chat));

            _databaseContext.Chats.Add(chat);
            _databaseContext.SaveChanges();

            var createdChat = _databaseContext.Chats.Find(chat.Id);
            return new Chat(createdChat);
        }

        [HttpPost("{chatId}")]
        public ActionResult<Chat> AddUserToChat(string chatId, UserUsername userUsername)
        {
            var chat = _databaseContext.Chats.Include(c => c.Users).FirstOrDefault(c => c.Id == chatId);
            if (chat == default)
            {
                return NotFound(new { ChatId = "Chat not found." });
            }

            var user = _databaseContext.Users.Find(userUsername.Username);
            if (user == null)
            {
                return NotFound(new { Username = "User not found." });
            }

            if (_databaseContext.UsersInChats.Find(chatId, userUsername.Username) != null)
            {
                return Conflict(new { Username = "User already exists in chat." });
            }

            var nameClaim = _identityService.GetUsername(HttpContext);

            if (chat.Users.FirstOrDefault(u => u.Username == nameClaim) == default)
            {
                return Unauthorized();
            }

            chat.Users.Add(new UserInChat(user, chat));
            _databaseContext.SaveChanges();
            var updatedChat = _databaseContext.Chats
                .Include(c => c.Messages)
                .ThenInclude(m => m.User)
                .Include(c => c.Users)
                .ThenInclude(uc => uc.User)
                .First(c => c.Id == chatId);

            return new Chat(updatedChat);
        }

        [HttpGet("{username}")]
        public ActionResult<IEnumerable<Chat>> GetAllChats(string username)
        {
            var nameClaim = _identityService.GetUsername(HttpContext);
            if (nameClaim != username)
            {
                return Unauthorized();
            }

            return _databaseContext.Users
                .Include(u => u.Chats)
                .ThenInclude(uc => uc.Chat)
                .ThenInclude(c => c.Users)
                .ThenInclude(uc => uc.User)
                .Include(u => u.Chats)
                .ThenInclude(uc => uc.Chat)
                .ThenInclude(c => c.Messages)
                .ThenInclude(m => m.User)
                .FirstOrDefault(u => u.Username == username)?.Chats
                .Select(uc => new Chat(uc.Chat))
                .ToList();
        }

        [HttpGet("{id}/messages")]
        public ActionResult<IEnumerable<ChatHistory>> GetAllMessages(string id)
        {
            var nameClaim = _identityService.GetUsername(HttpContext);

            // If requester is not part of the group
            if (_databaseContext.UsersInChats.Find(id, nameClaim) == null)
            {
                return Unauthorized();
            }

            var existingChat = _databaseContext.Chats
                .Include(c => c.Messages)
                .ThenInclude(message => message.User)
                .FirstOrDefault(c => c.Id == id);
            if (existingChat == default)
            {
                return NotFound();
            }

            return existingChat.Messages.OrderBy(m => m.SentAt).Select(m => new ChatHistory(m)).ToList();
        }

        [HttpGet("{id}/users")]
        public ActionResult<IEnumerable<PublicUser>> GetAllUsers(string id)
        {
            var nameClaim = _identityService.GetUsername(HttpContext);

            // If requester is not part of the group
            if (_databaseContext.UsersInChats.Find(id, nameClaim) == null)
            {
                return Unauthorized();
            }

            return _databaseContext.UsersInChats
                .Include(chat => chat.Chat)
                .Where(chat => chat.Chat.Id == id)
                .Include(chat => chat.User)
                .Select(chat => new PublicUser(chat.User))
                .ToList();
        }
    }
}
