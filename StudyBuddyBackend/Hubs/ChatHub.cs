using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using StudyBuddyBackend.Database.Contexts;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Hubs
{
    public class ChatHub : Hub
    {
        private class ActiveUser
        {
            internal User User { get; set; }
            internal string ConnectionId { get; set; }
            internal List<string> Groups { get; set; } = new List<string>();

            internal ActiveUser(User user, string connectionId, string groupName)
            {
                User = user;
                ConnectionId = connectionId;
                AddGroup(groupName);
            }

            internal void AddGroup(string groupName)
            {
                Groups.Add(groupName);
            }
        }

        private readonly DatabaseContext _databaseContext;
        private readonly List<ActiveUser> _userList = new List<ActiveUser>();

        public ChatHub(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public String GroupName;

        public async Task Connect(string username, string groupName)
        {
            var user = _databaseContext.Users.Find(username);
            if (user == null) return;

            Chat chat = _databaseContext.Chats.Find(groupName);
            if (chat == null)
            {
                chat = new Chat(groupName);
                GroupName = groupName;
                _databaseContext.Chats.Add(chat);
            }
            else
            {
                GroupName = groupName;
            }

            if (_databaseContext.Users.Find(user).Chats.All(c => c.Chat.GroupName != groupName))
            {
                chat.Users.Add(new UserChat(user, chat));
            }

            ActiveUser activeUser;
            if ((activeUser = _userList.Find(u => u.User.Username == username)) == null)
            {
                _userList.Add(new ActiveUser(user, Context.ConnectionId, groupName));
            }
            else
            {
                activeUser.AddGroup(groupName);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task SendMessage(string groupName, string messageText)
        {
            var message = new Message(_userList.Find(u => u.ConnectionId == Context.ConnectionId).User,
                messageText);
            _databaseContext.Messages.Add(message);
            _databaseContext.Chats.Find(groupName).Messages.Add(message);
            return Clients.Group(groupName).SendAsync(messageText);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return new Task(() =>
            {
                foreach (var group in _userList.Find(u => u.ConnectionId == Context.ConnectionId).Groups)
                {
                    Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                }
            });
        }
    }
}