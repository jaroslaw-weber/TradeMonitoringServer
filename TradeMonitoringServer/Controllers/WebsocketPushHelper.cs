using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TradeMonitoringServer.Controllers
{
    /// <summary>
    /// After connecting to this endpoint, server will push updates every second
    /// </summary>
    public class WebsocketPushHelper<T> where T:IJson, new()
    {
        public ILogger? Logger { get; set; }

        /// <summary>
        /// Every second this class will use this callback to create data and push to client
        /// </summary>
        public Func<T>? CreatePushData { get; set; }

        private HttpContext? context;

        private WebSocket? webSocket;

        public async Task OnConnectedToWebsocket(HttpContext context)
        {
            if (context == null) throw new System.NullReferenceException("context is null");
            this.context = context;
            Logger.LogInformation("accessing securities list");

            //only allows websockets
            if (!context.WebSockets.IsWebSocketRequest)
            {
                OnInvalidRequest();
                return;
            }
            webSocket = await WaitForSocketConnection();
            Logger.LogInformation("connected to websocket");

            await PushUpdateEverySecond();

        }

        private async Task<WebSocket> WaitForSocketConnection()
        {
            return await context.WebSockets.AcceptWebSocketAsync();
        }

        private void OnInvalidRequest() => context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// Every second gets positions data and send them to client
        /// </summary>
        private async Task PushUpdateEverySecond()
        {
            while (true)
            {
                await PushDataToClient();
                await WaitForNextUpdate();
            }
        }

        /// <summary>
        /// Send position info to connected client
        /// </summary>
        private async Task PushDataToClient()
        {
            if (webSocket == null) throw new System.NullReferenceException("webSocket is null");
            //in case CreatePushData was not set
            if (this.CreatePushData == null) throw new System.NullReferenceException("failed to create data to push");
            var message = this.CreatePushData();
            await PushMessageToClient(message, webSocket, Logger);
        }

        private async Task WaitForNextUpdate() => await Task.Delay(1000);


        /// <summary>
        /// Convert data to Json and send to client
        /// </summary>
        private static async Task PushMessageToClient(IJson response, WebSocket webSocket, ILogger logger)
        {
            var message = response.ToJson();

            logger.LogInformation("sending message: " + message);

            var content = StringToArraySegment(message);
            await webSocket.SendAsync(content, WebSocketMessageType.Text, true, CancellationToken.None);

            logger.LogDebug("sent message to client!" + message);
        }

        private static ArraySegment<byte> StringToArraySegment(string input)
        {
            var encoded = Encoding.UTF8.GetBytes(input);

            return new ArraySegment<Byte>(encoded, 0, encoded.Length);

        }
    }

}