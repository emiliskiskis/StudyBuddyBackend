using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StudyBuddyBackend.Database;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Hubs
{
    public class ChatHub : Hub<IHubClient>
    {
        private readonly DatabaseContext _databaseContext;

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

        public ChatHub(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task Connect(string username)
        {
            var user = _databaseContext.Users.Include(u => u.Chats).ThenInclude(chat => chat.Chat)
                .FirstOrDefault(u => u.Username == username);
            if (user == null) return;
            foreach (var chat in user.Chats)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chat.Chat.Id);
            }

            _activeUsers.Add(new ActiveUser(Context.ConnectionId, username));
        }

        public async Task ConnectOther(string username, string chatId)
        {
            var activeUser = _activeUsers.FirstOrDefault(user => user.Username == username);
            if (activeUser == default) return;

            var userChat = _databaseContext.UsersInChats.Include(chat => chat.User).Include(chat => chat.Chat)
                .FirstOrDefault(u => u.User.Username == username && u.Chat.Id == chatId);
            if (userChat == default) return;

            await Clients.User(activeUser.ConnectionId).ReceiveChat(chatId);
        }

        public async Task SendMessage(string username, string chatId, string messageId, string messageText)
        {
            _databaseContext.Chats.Find(chatId).Messages
                .Add(new Message(messageId, _databaseContext.Users.Find(username), messageText));
            _databaseContext.SaveChanges();
            await Clients.Group(chatId).ReceiveMessage(username, chatId, messageId, messageText, DateTime.Now);
        }
    }
}
