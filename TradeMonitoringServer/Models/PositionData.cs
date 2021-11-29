using System;

namespace TradeMonitoringServer
{
    public class PositionData : IClone<PositionData>
    {
        public int Id { get; private set; }

        public int Price { get; private set; }

        public string Ticker { get; private set; } = string.Empty;

        /// <summary>
        /// How many shares of the position have right now
        /// </summary>
        public int CurrentQuantity { get; private set; }

        /// <summary>
        /// How many shares of the position had at the start of the day
        /// </summary>
        public int DayStartQuantity { get; private set; }

        /// <summary>
        /// How many shares of the position were sold/bought
        /// </summary>
        public int QuantityTraded { get; private set; }

        public PositionData Clone()
        {
            return (PositionData)this.MemberwiseClone();
        }

        /// <summary>
        /// Creates dummy data
        /// </summary>
        public static PositionData CreateDummy(int id)
        {
            var position = new PositionData();
            position.Id = id;
            position.Ticker = DummyDataHelper.FakeTickerNames[id - 1];
            position.CurrentQuantity = DummyDataHelper.GetRandomPositonQuantity();
            position.DayStartQuantity = position.CurrentQuantity;
            position.Price = DummyDataHelper.GetRandomPositionPrice();
            return position;
        }


        /// <summary>
        /// Simulate price change
        /// </summary>
        public void RandomlyChangePrice()
        {
            Price = DummyDataHelper.GetRandomPrice(Price);
        }

        /// <summary>
        /// Apply quantity change after trade
        /// </summary>
        public void ApplyTrade(TradeData trade)
        {
            QuantityTraded += trade.Quantity;
            switch (trade.TradeType)
            {
                case TradeType.Buy:
                    CurrentQuantity += trade.Quantity;
                    return;
                case TradeType.Sell:
                    CurrentQuantity -= trade.Quantity;
                    return;
                default: throw new System.SystemException("invalid trade type");

            }
        }

    }
}
