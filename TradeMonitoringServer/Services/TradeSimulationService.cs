using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TradeMonitoringServer
{
    /// <summary>
    /// Service responsible for running TradeSimulation
    /// </summary>
    public class TradeSimulationService
    {
        public async Task StartSimulation()
        {
            TradeSimulation.Instance = new TradeSimulation();
            while (true)
            {
                int waitTime = TradeSimulation.Instance.GetRandomWaitTime();
                await Task.Delay(waitTime);
                TradeSimulation.Instance.SimulateTrade();
            }
        }
    }
}