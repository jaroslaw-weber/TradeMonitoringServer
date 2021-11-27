using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TradeMonitoringServer.Controllers
{

    [ApiController]
	[Route("[controller]")]
	public class PositionListController : ControllerBase
	{
		private readonly ILogger<PositionListController> _logger;


		public PositionListController(ILogger<PositionListController> logger)
		{
			_logger = logger;
		}

		/// <summary>
        /// After connecting to this socket, servers push position list to client every second
        /// </summary>
		[HttpGet("/position-list")]
		public async Task Get()
        {
            _logger.LogInformation("accessing securities list");

            //only allows websockets
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                OnInvalidRequest();
                return;
            }
            WebSocket webSocket = await WaitForSocketConnection();
            _logger.LogInformation("connected to websocket");

            await PushUpdateEverySecond(HttpContext, webSocket);

        }

        private async Task<WebSocket> WaitForSocketConnection()
        {
            return await HttpContext.WebSockets.AcceptWebSocketAsync();
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

		/// <summary>
        /// Send position info to connected client
        /// </summary>
        private async Task PushDataToClient(WebSocket webSocket)
        {
			var response = new PositionListMessage();
            await response.PushMessageToClient(webSocket, _logger);
        }

        private async Task WaitForNextUpdate() => await Task.Delay(1000);
		


	}

}