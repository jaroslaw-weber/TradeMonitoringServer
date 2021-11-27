

using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeMonitoringServer
{
    /// <summary>
    /// Simulating selling and buying positions
    /// Saves trades history, current states of postitions and state at the start of the day
    /// </summary>
    public class TradeSimulation
    {
        public Random random = new Random();

        List<TradeData> trades = new();

        /// <summary>
        /// state of positions at the start of the day (before any trades)
        /// key: position id
        /// </summary>
        Dictionary<int, PositionData> startOfTheDayState = new();

        /// <summary>
        /// state of positions (currently)
        /// key: position id
        public Dictionary<int, PositionData> CurrentPositionsState { get; set; } = new();

        string[] FakeTickerNames = new[] { "AAPL", "INTL", "MSFT", "AMD", "LOGI", "CRSR", "NVDA", "CAT" };

        /// <summary>
        /// How many positions are there. For the sake of simplicity it is constant.
        /// </summary>
        /// <returns></returns>
        private int GetPositionsLength() => FakeTickerNames.Length;

        public TradeSimulation()
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
            trades.Add(trade);
            RecalculateCurrentState();
        }

        /// <summary>
        /// calculate current state of positions based on trades
        /// </summary>
        private void RecalculateCurrentState()
        {
            //copy start of the day positions
            CurrentPositionsState = startOfTheDayState.ToDictionary(x => x.Key, y => (PositionData)y.Value.Clone());
            //apply trades
            foreach (var trade in trades)
            {
                var position = CurrentPositionsState[trade.PositionId];
                switch (trade.TradeType)
                {
                    case TradeType.Buy:
                        position.CurrentQuantity += trade.Quantity;
                        continue;
                    case TradeType.Sell:
                        position.CurrentQuantity -= trade.Quantity;
                        continue;
                    default: throw new System.SystemException("invalid trade type");

                }
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
            return CurrentPositionsState[index];
        }
    }
}