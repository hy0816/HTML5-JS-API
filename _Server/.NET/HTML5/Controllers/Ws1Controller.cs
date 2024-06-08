using Microsoft.AspNetCore.Mvc;
using System.Text;
using System;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using HTML5.Models;

namespace HTML5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Ws1Controller : ControllerBase
    {

        //�����Ҧ��s�u���H
        static ConcurrentDictionary<int, WebSocket> WebSockets = new ConcurrentDictionary<int, WebSocket>();

        public Ws1Controller()
        {

        }

        [HttpGet("chatWs")]
        public async Task chatWs()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                WebSockets.TryAdd(webSocket.GetHashCode(), webSocket);  //�N�s���i�Ӫ��ϥΪ̥[��WebSockets���X(ConcurrentDictionary)
                await ProcessWebSocket(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        //�w�qProcessWebSocket�禡
        private async Task ProcessWebSocket(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4]; //�إߤ@��4k�j�p��RAM�Ŷ��A�ΨӦs��n�ǰe�����

            //�N�������ƶ�ibuffer���A�����������B�z
            var res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!res.CloseStatus.HasValue)
            {
                string json = Encoding.UTF8.GetString(buffer, 0, res.Count);
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                };
                MsgObj? receivedMsg = JsonSerializer.Deserialize<MsgObj>(json, options);
                if (receivedMsg != null)
                {
                    Broadcast(receivedMsg); //�����쪺��ƶǵ�Broadcase�ۭq�禡�A�b���禡���s�����Ҧ��s�u���ϥΪ�
                }
                res = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            //websocket����
            await webSocket.CloseAsync(res.CloseStatus.Value, res.CloseStatusDescription, CancellationToken.None);
            //�qWebSockets���X(ConcurrentDictionary)�������u�ϥΪ�
            WebSockets.TryRemove(webSocket.GetHashCode(), out var removed);
        }

        //�w�qBroadcase�禡
        private void Broadcast(MsgObj msgObj)
        {
            //����B��
            Parallel.ForEach(WebSockets.Values, async (webSocket) =>
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    string msg = msgObj.User + " ��: " + msgObj.Message;
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            });
        }

    }
}
