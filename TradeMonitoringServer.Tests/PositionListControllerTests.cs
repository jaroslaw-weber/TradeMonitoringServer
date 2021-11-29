using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace TradeMonitoringServer.Tests
{
    public class PositionListControllerTests
    {
        private  IHost server;
        private  ClientWebSocket client;
        private  ILogger<PositionListControllerTests> logger;
        private CancellationToken serverCancellationToken;
        private CancellationToken clientCancellationToken;
        private const int port = 6000;

        public PositionListControllerTests()
        {
            //create logger
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            this.logger = loggerFactory.CreateLogger<PositionListControllerTests>();
            logger.LogInformation("test: created logger");

            //setup server
            serverCancellationToken = new CancellationToken();
            var builder = Program.CreateHostBuilder(new string[] { });
            server = builder.Build();
            server.RunAsync(serverCancellationToken);
            //setup fake client
            client = new ClientWebSocket();

        }

        /// <summary>
        /// Connect to server through websocket
        /// </summary>
        private async Task ConnectToServer()
        {
            logger.LogInformation("test: connecting to server");
            var uriString = $"ws://localhost:{port}/positions";
            var uri = new System.Uri(uriString);
            logger.LogInformation("test: connecting to server: "+uriString);
            await client.ConnectAsync(uri, clientCancellationToken);
            logger.LogInformation("test: connected to server");
            //wait for data from server

        }

        /// <summary>
        /// Wait for data pushed from server
        /// </summary>
        private async Task<string> WaitForServerMessage()
        { 
            var buffer = CreateBuffer();
            var received = await client.ReceiveAsync(buffer, CancellationToken.None);
            logger.LogInformation("test: received message");
            var receivedAsText = Encoding.UTF8.GetString(buffer.Array, 0, received.Count);
            return receivedAsText;
        }

        /// <summary>
        /// Test if data is pushed to client is valid
        /// </summary>
        [Fact(Timeout = 3000)]
        public async Task PushDataTests()
        {
            await ConnectToServer();
            var receivedAsText = await WaitForServerMessage();
            var messageParsed = JsonSerializer.Deserialize<PositionListMessage>(receivedAsText);

            //test if data is valid / parsed succesfully
            Assert.True(messageParsed.Timestamp.Year > 2000, "timestamp is invalid");
            Assert.True(messageParsed.Positions.Length > 0, "empty position list");

            //test if getting more than one message
            var message2 = await WaitForServerMessage();

            Assert.True(!string.IsNullOrEmpty(message2), "received only one message");
        }

        private ArraySegment<byte> CreateBuffer()
        {
            return new ArraySegment<byte>(new byte[1024]);
        }
    }

}
