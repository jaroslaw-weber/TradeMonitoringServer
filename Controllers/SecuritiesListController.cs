using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace stock_price_app_server.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class SecuritiesListController : ControllerBase
	{
		private readonly ILogger<SecuritiesListController> _logger;

		public SecuritiesListController(ILogger<SecuritiesListController> logger)
		{
			_logger = logger;
		}

		[HttpGet("/securities-list")]
		public async Task Get()
		{
			_logger.Log(LogLevel.Information, "accessing securities list");

			if (HttpContext.WebSockets.IsWebSocketRequest)
			{
				using WebSocket webSocket = await
								   HttpContext.WebSockets.AcceptWebSocketAsync();

				_logger.Log(LogLevel.Information, "connected to websocket");
				await StartSendingData(HttpContext, webSocket);
			}
			else
			{
				HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
			}
		}
		private async Task StartSendingData(HttpContext context, WebSocket webSocket)
		{
			var buffer = new byte[1024 * 4];
			WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			_logger.Log(LogLevel.Information, "received message from client");

			while (true)
			{
				var message = DateTime.Now.ToString();
				var encoded = Encoding.UTF8.GetBytes(message);

				var content = new ArraySegment<Byte>(encoded, 0, encoded.Length);
				await webSocket.SendAsync(content, WebSocketMessageType.Text, true, CancellationToken.None);
				/*var content = new ArraySegment<byte>(buffer, 0, result.Count, result.MessageType, result.EndOfMessage, CancellationToken.None);
				await webSocket.SendAsync(content);
				*/
				_logger.Log(LogLevel.Information, "sent message to client!");

				await Task.Delay(1000);
			}


			await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
		}

	}

}