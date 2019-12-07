using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Hubs
{
    public class ChatHub : Hub<IHubClient>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        private readonly ActiveUserService _activeUserService;

        public ChatHub(DatabaseContext databaseContext, ILogger<ChatHub> logger, ActiveUserService activeUserService)
        {
            _activeUserService = activeUserService;
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Connect(string username)
        {
            var user = _databaseContext.Users
                .Include(u => u.Chats)
                .FirstOrDefault(u => u.Username == username);
            if (user == default) return;
            foreach (var userInChat in user.Chats)
            {
                _logger.LogInformation(userInChat.ChatId);
                await Groups.AddToGroupAsync(Context.ConnectionId, userInChat.ChatId);
            }

            _activeUserService.ActiveUsers.Add(new ActiveUser(Context.ConnectionId, username));
        }

        public async Task ConnectOther(string username, string chatId)
        {
            var activeUser = _activeUserService.ActiveUsers.FirstOrDefault(user => user.Username == username);
            if (activeUser == default) return;

            var userChat = _databaseContext.UsersInChats.Find(chatId, username);
            if (userChat == null) return;

            await Clients.User(activeUser.ConnectionId).ReceiveChat(chatId);
        }

        public async Task SendMessage(string username, string chatId, string text, int tempId)
        {
            _databaseContext.Chats.Find(chatId).Messages.Add(new Message(username, text));
            _databaseContext.SaveChanges();
            await Clients.OthersInGroup(chatId).ReceiveMessage(username, chatId, text, DateTime.Now);
            await Clients.Caller.MessageSuccess(chatId, tempId);
        }
    }
}
