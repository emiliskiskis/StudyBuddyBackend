using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace StudyBuddyBackend.Hubs
{
    public class ChatHub : Hub
    {
        private class ActiveUser
        {
            internal string Username { get; }
            internal string ConnectionId { get; }
            internal List<string> Groups { get; } = new List<string>();

            internal ActiveUser(string username, string connectionId, string groupName)
            {
                Username = username;
                ConnectionId = connectionId;
                AddGroup(groupName);
            }

            internal void AddGroup(string groupName)
            {
                Groups.Add(groupName);
            }
        }

        private readonly List<ActiveUser> _userList = new List<ActiveUser>();

        public async Task Connect(string username, string groupName)
        {
            ActiveUser activeUser;
            if ((activeUser = _userList.Find(u => u.Username == username)) == null)
            {
                _userList.Add(new ActiveUser(username, Context.ConnectionId, groupName));
            }
            else
            {
                activeUser.AddGroup(groupName);
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task SendMessage(string groupName, string messageText)
        {
            return Clients.Group(groupName).SendAsync(messageText);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return new Task(async () =>
            {
                var user = _userList.Find(u => u.ConnectionId == Context.ConnectionId);
                foreach (var group in user.Groups)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
                }

                _userList.Remove(user);
            });
        }
    }
}
