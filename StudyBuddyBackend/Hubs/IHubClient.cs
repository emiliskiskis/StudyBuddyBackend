using System;
using System.Threading.Tasks;

namespace StudyBuddyBackend.Hubs
{
    public interface IHubClient
    {
        Task ReceiveChat(string chatId);
        Task ReceiveMessage(string username, string chatId, string messageText, DateTime sentAt);
        Task MessageSuccess(string chatId, int tempId);
    }
}
