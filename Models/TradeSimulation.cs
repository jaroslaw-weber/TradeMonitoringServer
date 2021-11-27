

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

        List<TradeData> trades = new();

        /// <summary>
        /// state of positions at the start of the day (before any trades)
        /// key: position id
        /// </summary>
        PositionDataDictionary startOfTheDayState = new();

        /// <summary>
        /// state of positions (currently)
        /// key: position id
        public PositionDataDictionary CurrentPositionsState { get; set; } = new();

        string[] FakeTickerNames = new[] { "AAPL", "INTL", "MSFT", "AMD", "LOGI", "CRSR", "NVDA", "CAT" };

        /// <summary>
        /// How many positions are there. For the sake of simplicity it is constant.
        /// </summary>
        /// <returns></returns>
        private int GetPositionsLength() => FakeTickerNames.Length;

        private ILogger<TradeSimulation>? logger;


        private void Initialize()
        {

            var length = GetPositionsLength();
            for (int i = 0; i < length; i++)
            {
                int id = i + 1;
                var position = CreateFakePosition(id);
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

        private PositionData CreateFakePosition(int id)
        {
            var position = new PositionData();
            position.Id = id;
            position.Ticker = FakeTickerNames[id - 1];
            position.CurrentQuantity = GetRandomPositonQuantity();
            position.Price = GetRandomPositionPrice();
            return position;
        }

        private int GetRandomPositonQuantity() => random.Next(300, 2000);

        private int GetRandomPositionPrice() => random.Next(50, 300);

        /// <summary>
        /// Creates new fake trade and updates current state
        /// </summary>
        public void SimulateTrade()
        {
            var trade = GetNewFakeTrade();
            logger?.LogInformation("new trade: " + trade.ToJson());
            trades.Add(trade);

            RecalculateCurrentState();
        }

        /// <summary>
        /// calculate current state of positions based on trades
        /// </summary>
        private void RecalculateCurrentState()
        {
            //copy start of the day positions
            CurrentPositionsState = startOfTheDayState.Clone();
            //apply trades
            foreach (var trade in trades)
            {
                CurrentPositionsState.ApplyTrade(trade);
            }

        }

        private TradeData GetNewFakeTrade()
        {
            var position = GetRandomPosition();
            var trade = new TradeData();
            trade.Id = trades.Count;
            trade.PositionId = position.Id;
            trade.TradeType = GetRandomTradeType(position);
            trade.Quantity = GetRandomTradeQuantity(position, trade.TradeType);
            return trade;


        }

        private TradeType GetRandomTradeType(PositionData position)
        {
            //only buy available if there is 0 quantity
            if (position.CurrentQuantity == 0) return TradeType.Buy;
            //otherwise random
            double randomDouble = random.NextDouble();
            return randomDouble > 0.5 ? TradeType.Buy : TradeType.Sell;
        }

        private int GetRandomTradeQuantity(PositionData position, TradeType tradeType)
        {
            switch (tradeType)
            {
                case TradeType.Sell:
                    //check if data is valid
                    if (position.CurrentQuantity <= 0)
                        throw new System.DataMisalignedException("0 quantity, cannot sell this position");
                    return random.Next(1, position.CurrentQuantity);
                case TradeType.Buy:
                    return random.Next(1, 300);
                default:
                    throw new System.Exception("invalid trade type");
            }

        }

        private PositionData GetRandomPosition()
        {
            var index = random.Next(0, startOfTheDayState.Count);
            int id = index + 1;
            return CurrentPositionsState[id];
        }
    }
}