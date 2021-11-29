

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TradeMonitoringServer
{
    //todo refactor: split simulation part from calculations

    /// <summary>
    /// Simulating selling and buying positions
    /// Saves trades history, current states of postitions and state at the start of the day
    /// </summary>
    public class TradeSimulation
    {
        /// <summary>
        /// Creating only one instance of simulation,
        /// so each user connecting to the server will have same result.
        /// </summary>
        public static TradeSimulation? Instance;

        public Random random = new Random();

        public List<TradeData> Trades { get; set; } = new();

        /// <summary>
        /// state of positions at the start of the day (before any trades)
        /// key: position id
        /// </summary>
        PositionDataDictionary startOfTheDayState = new();

        /// <summary>
        /// state of positions (currently)
        /// key: position id
        public PositionDataDictionary CurrentPositionsState { get; set; } = new();


        /// <summary>
        /// How many positions are there. For the sake of simplicity it is constant.
        /// </summary>
        /// <returns></returns>
        private int GetPositionsLength() => DummyDataHelper.FakeTickerNames.Length;

        private ILogger<TradeSimulation>? logger;


        private void Initialize()
        {

            var length = GetPositionsLength();
            for (int i = 0; i < length; i++)
            {
                int id = i + 1;
                var position = PositionData.CreateDummy(id);
                startOfTheDayState[id] = position;
                CurrentPositionsState[id] = position;
            }
        }

        public TradeSimulation()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            this.logger = loggerFactory.CreateLogger<TradeSimulation>();
            logger?.LogInformation("created trade simulation logger");
            Initialize();
        }

        /// <summary>
        /// Randomizes wait time between trades
        /// </summary>
        /// <returns>Miliseconds to wait</returns>
        public int GetRandomWaitTime() => random.Next(100, 900);

        /// <summary>
        /// Creates new fake trade and updates current state
        /// </summary>
        public void SimulateTrade()
        {
            var trade = GetNewFakeTrade();
            logger?.LogInformation("new trade: " + trade.ToJson());
            Trades.Add(trade);

            RecalculateCurrentState();
        }

        /// <summary>
        /// Calculate current state of positions based on trades
        /// </summary>
        private void RecalculateCurrentState()
        {
            //copy start of the day positions
            CurrentPositionsState = startOfTheDayState.Clone();
            //apply trades
            foreach (var trade in Trades)
            {
                CurrentPositionsState.ApplyTrade(trade);
            }
            //simulate price change
            ApplyRandomPriceFluctuations();
        }

        /// <summary>
        /// Simulate price change
        /// </summary>
        private void ApplyRandomPriceFluctuations()
        {
            foreach(var pair in CurrentPositionsState)
            {
                pair.Value.RandomlyChangePrice();
            }
        }

        private TradeData GetNewFakeTrade()
        {
            var position = GetRandomPosition();
            return TradeData.CreateDummy(Trades.Count, position);
        }


        private PositionData GetRandomPosition()
        {
            var index = random.Next(0, startOfTheDayState.Count);
            int id = index + 1;
            return CurrentPositionsState[id];
        }
    }
}