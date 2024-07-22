using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace UserChatManagement
{
    public class NotificationHub : Hub
    {
        public async Task NotifyNewUser(string userName)
        {
            await Clients.All.SendAsync("ReceiveNewUserNotification", userName);
        }

        public async Task RequestHelp(string userName)
        {
            await Clients.All.SendAsync("ReceiveHelpRequest", userName);
        }
    }
}
