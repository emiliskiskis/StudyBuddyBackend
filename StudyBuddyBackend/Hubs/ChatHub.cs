using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using StudyBuddyBackend.Database;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Hubs
{
    public class ChatHub : Hub<IHubClient>
    {
        private readonly DatabaseContext _databaseContext;
        private readonly ILogger _logger;

        public ChatHub(DatabaseContext databaseContext, ILogger<ChatHub> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Connect(string username, string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task SendMessage(string username, string groupName, string messageText)
        {
            _logger.LogInformation($"{username} ({groupName}): {messageText}");
            _databaseContext.Chats.Find(groupName).Messages
                .Add(new Message(_databaseContext.Users.Find(username), messageText));
            _databaseContext.SaveChanges();
            await Clients.Group(groupName).ReceiveMessage(username, groupName, messageText, DateTime.Now);
        }
    }
}
