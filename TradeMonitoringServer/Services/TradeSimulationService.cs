using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace TradeMonitoringServer
{
    /// <summary>
    /// Service responsible for running TradeSimulation
    /// </summary>
    public static class TradeSimulationService
    {
        private static bool isRunning { get; set; }

        public static async Task StartSimulation()
        {
            isRunning = true;
            TradeSimulation.Instance = new TradeSimulation();
            while (isRunning)
            {
                int waitTime = TradeSimulation.Instance.GetRandomWaitTime();
                await Task.Delay(waitTime);
                TradeSimulation.Instance.SimulateTrade();
            }
        }

        public static void StopSimulation()
        {
            isRunning = false;
        }
    }
}