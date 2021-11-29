using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace TradeMonitoringServer.Controllers
{
    /// <summary>
    /// API endpoint: push trade list to client every second
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TradeListController : ControllerBase
    {
        private readonly ILogger<TradeListController> logger;

        private WebsocketPushHelper<TradeListMessage> websocketPushHelper = new();

        public TradeListController(ILogger<TradeListController> logger)
        {
            this.logger = logger;
            this.websocketPushHelper.Logger = logger;
            //each second load data about trades and create new message which will be send to client
            this.websocketPushHelper.CreatePushData = () => new TradeListMessage();
        }

        /// <summary>
        /// After connecting to this endpoint, servers push trades list to client every second
        /// </summary>
        [HttpGet("/trades")]
        public async Task Get()
        {
            await websocketPushHelper.OnConnectedToWebsocket(HttpContext);
        }
    }

}