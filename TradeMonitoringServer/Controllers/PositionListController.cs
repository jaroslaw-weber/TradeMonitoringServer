using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TradeMonitoringServer.Controllers
{
	/// <summary>
	/// API endpoint: push position list to client every second
	/// </summary>
	[ApiController]
	[Route("[controller]")]
	public class PositionListController : ControllerBase
	{
		private readonly ILogger<PositionListController> logger;

        private WebsocketPushHelper<PositionListMessage> websocketPushHelper = new ();

		public PositionListController(ILogger<PositionListController> logger)
		{
			this.logger = logger;
			this.websocketPushHelper.Logger = logger;
			//each second load data about positions and create new message which will be send to client
			this.websocketPushHelper.CreatePushData = () => new PositionListMessage();
		}

		/// <summary>
		/// After connecting to this endpoint, servers push position list to client every second
		/// </summary>
		[HttpGet("/positions")]
		public async Task Get()
		{
			await websocketPushHelper.OnConnectedToWebsocket(HttpContext);

		}
	}

}