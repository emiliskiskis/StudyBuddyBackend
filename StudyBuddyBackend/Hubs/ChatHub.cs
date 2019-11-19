using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StudyBuddyBackend.Database;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Hubs
{
    public class ChatHub : Hub<IHubClient>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        private class ActiveUser
        {
            internal string ConnectionId { get; }
            internal string Username { get; }

            internal ActiveUser(string connectionId, string username)
            {
                ConnectionId = connectionId;
                Username = username;
            }
        }

        private readonly ICollection<ActiveUser> _activeUsers = new List<ActiveUser>();

        public ChatHub(DatabaseContext databaseContext, ILogger<ChatHub> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Connect(string username)
        {
            var userChats = _databaseContext.Users.Include(user => user.Chats).ThenInclude(chat => chat.Chat).FirstOrDefault(user => user.Username == username)?.Chats;
            foreach (var chat in userChats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Chat.Id);
            }
            _activeUsers.Add(new ActiveUser(Context.ConnectionId, username));
        }

        public async Task ConnectOther(string username, string chatId)
        {
            var activeUser = _activeUsers.FirstOrDefault(user => user.Username == username);
            if (activeUser == default) return;

            var userChat = _databaseContext.UsersInChats.Include(uchat => uchat.User).Include(uchat => uchat.Chat).FirstOrDefault(u => u.User.Username == username && u.Chat.Id == chatId);
            if (userChat == default) return;

            await Clients.User(activeUser.ConnectionId).ReceiveChat(chatId);
        }

        public async Task SendMessage(string username, string chatId, string messageText)
        {
            _logger.LogInformation($"{username} ({chatId}): {messageText}");
            _databaseContext.Chats.Find(chatId).Messages
                .Add(new Message(_databaseContext.Users.Find(username), messageText));
            _databaseContext.SaveChanges();
            await Clients.Group(chatId).ReceiveMessage(username, chatId, messageText, DateTime.Now);
        }
    }
}
