using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Net;
using Microsoft.Extensions.Logging;

namespace stock_price_app_server.Controllers;

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
			await Echo(HttpContext, webSocket);
		}
		else
		{
			HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
		}
	}
	private async Task Echo(HttpContext context, WebSocket webSocket)
	{
		var buffer = new byte[1024 * 4];
		WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

		_logger.Log(LogLevel.Information, "received message from client");
		while (!result.CloseStatus.HasValue)
		{
			await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
			_logger.Log(LogLevel.Information, "sent message to client!");
			result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
		}
		await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
	}

}
