using Xunit;
using System.Linq;

namespace TradeMonitoringServer.Tests
{
    public class TradeSimulationTests
    {
        /// <summary>
        /// Check if positions were automatically created
        /// </summary>
        [Fact]
        public void CurrentPositionsNotNull()
        {
            TradeSimulation.Instance = new TradeSimulation();
            Assert.NotNull(TradeSimulation.Instance.CurrentPositionsState);
        }

        /// <summary>
        /// Simulate one trade and check if anything changed
        /// </summary>
        [Fact]
        public void SimulateSingleTrade()
        {
            TradeSimulation.Instance = new TradeSimulation();

            var i = TradeSimulation.Instance;
            var previousState = i.CurrentPositionsState.Clone();
            i.SimulateTrade();
            var newState = i.CurrentPositionsState;
            bool stateChanged = false;
            foreach (var pair in newState.Zip(previousState))
            {
                if (pair.First.Value.CurrentQuantity == pair.Second.Value.CurrentQuantity)
                    continue;
                stateChanged = true;
            }
            Assert.True(stateChanged, "nothing changed, simulation failed");

        }
    }
}
