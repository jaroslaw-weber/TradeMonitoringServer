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
            var simulation = new TradeSimulation();
            Assert.NotNull(simulation.CurrentPositionsState);
        }

        /// <summary>
        /// Simulate one trade and check if anything changed
        /// </summary>
        [Fact]
        public void SimulateSingleTrade()
        {
            //simulate single trade
            var simulation = new TradeSimulation();

            var previousState = simulation.CurrentPositionsState.Clone();
            simulation.SimulateTrade();
            var newState = simulation.CurrentPositionsState;

            //set flag
            bool stateChanged = false;

            //check if anything changed
            foreach (var pair in newState.Zip(previousState))
            {
                if (pair.First.Value.CurrentQuantity == pair.Second.Value.CurrentQuantity)
                    continue;
                stateChanged = true;
            }
            Assert.True(stateChanged, "nothing changed, simulation failed");

        }

        /// <summary>
        /// Test if wait time between simulated trades is not too long and not too short
        /// </summary>
        [Fact]
        public void TestWaitTime()
        {
            var simulation = new TradeSimulation();
            for(int i=0;i<10;i++)
            {
                var waitTime = simulation.GetRandomWaitTime();
                Assert.InRange(waitTime, 1, 3000);
            }

        }
    }

}
