using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;
using Microsoft.AspNetCore.Http;
using TestWebAppDotNet;
using System.Text.Json;

namespace TradeMonitoringServer.Controllers
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

		/// <summary>
        /// After connecting to this socket, servers push position list to client every second
        /// </summary>
		[HttpGet("/securities-list")]
		public async Task Get()
		{
			_logger.Log(LogLevel.Information, "accessing securities list");

			//only allows websockets
			if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
				OnInvalidRequest();
				return;
            }

			using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

			_logger.Log(LogLevel.Information, "connected to websocket");

			await PushUpdateEverySecond(HttpContext, webSocket);
			
		}

		private void OnInvalidRequest()=> HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

		/// <summary>
		/// Every second gets positions data and send them to client
		/// </summary>
		private async Task PushUpdateEverySecond(HttpContext context, WebSocket webSocket)
		{
			while (true)
			{
				await PushDataToClient(webSocket);
				await WaitForNextUpdate();
			}
		}

        private async Task PushDataToClient(WebSocket webSocket)
        {
			var response = new PositionListMessage();
            await response.PushMessageToClient(webSocket, _logger);
        }

        private async Task WaitForNextUpdate() => await Task.Delay(1000);
		


	}

}