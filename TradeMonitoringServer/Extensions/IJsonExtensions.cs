using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TradeMonitoringServer
{
    public static class IJsonExtensions
    {
        /// <summary>
        /// Convert data to Json and send to client
        /// </summary>
        public static async Task PushMessageToClient(this IJson response, WebSocket webSocket, ILogger logger)
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