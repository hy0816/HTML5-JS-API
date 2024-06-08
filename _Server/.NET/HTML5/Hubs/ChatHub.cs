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

            //if (ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault() == null)
            //{
            //    ConnIDList.Add(Context.ConnectionId);
            //}
            //// ��s�s�u ID �C��
            //string jsonString = JsonConvert.SerializeObject(ConnIDList);
            //await Clients.All.SendAsync("UpdList", jsonString);

            //// ��s�ӤH ID
            //await Clients.Client(Context.ConnectionId).SendAsync("UpdSelfID", Context.ConnectionId);

            //// ��s��Ѥ��e
            //await Clients.All.SendAsync("UpdContent", "�s�s�u ID: " + Context.ConnectionId);

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
            //string id = ConnIDList.Where(p => p == Context.ConnectionId).FirstOrDefault();
            //if (id != null)
            //{
            //    ConnIDList.Remove(id);
            //}
            //// ��s�s�u ID �C��
            //string jsonString = JsonConvert.SerializeObject(ConnIDList);
            //await Clients.All.SendAsync("UpdList", jsonString);

            //// ��s��Ѥ��e
            //await Clients.All.SendAsync("UpdContent", "�w���u ID: " + Context.ConnectionId);

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
            //if (string.IsNullOrEmpty(user))
            //{
            //}
            //else
            //{
            //    // �����H
            //    await Clients.Client(sendToID).SendAsync("UpdContent", selfID + " �p�T�V�A��: " + message);

            //    // �o�e�H
            //    await Clients.Client(Context.ConnectionId).SendAsync("UpdContent", "�A�V " + sendToID + " �p�T��: " + message);
            //}
        }

    }
}
