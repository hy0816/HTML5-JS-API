using System.Net.WebSockets;
using Microsoft.AspNetCore.SignalR;

namespace HTML5.Hubs
{
    public class ChatHub : Hub
    {

        /// <summary>
        /// �s�u�ƥ�
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UpdContent", "�s�s�u ID: " + Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// ���u�ƥ�
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.SendAsync("UpdContent", "�w���u ID: " + Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }

        /// <summary>
        /// �ǻ��T��
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("UpdContent", user + " ��: " + message);
        }

    }
}
