using System;

namespace TradeMonitoringServer
{
    /// <summary>
    /// Utility class containing usefull functions for creating dummy data
    /// </summary>
    public static class DummyDataHelper
    {
        private static Random Random => Utility.Random;

        public static readonly string[] FakeTickerNames = new[] { "AAPL", "INTL", "MSFT", "AMD", "LOGI", "CRSR", "NVDA", "CAT" };

        public static int GetRandomPrice(int oldPrice)
        {
            //price not always change!
            if (Random.NextDouble() > 0.5) return oldPrice;
            //dont let price be negative
            if (oldPrice < 100) return oldPrice + Random.Next(2, 4);
            //randomly change price a bit;
            return oldPrice + Random.Next(-2, 2);
        }
        

        public static int GetRandomPositonQuantity() => Random.Next(300, 2000);

        public static int GetRandomPositionPrice() => Random.Next(50, 300);

        public static TradeType GetRandomTradeType(PositionData position)
        {
            //only buy available if there is 0 quantity
            if (position.CurrentQuantity == 0) return TradeType.Buy;
            //otherwise random
            double randomDouble = Random.NextDouble();
            return randomDouble > 0.5 ? TradeType.Buy : TradeType.Sell;
        }

        public static int GetRandomTradeQuantity(PositionData position, TradeType tradeType)
        {
            switch (tradeType)
            {
                case TradeType.Sell:
                    //check if data is valid
                    if (position.CurrentQuantity <= 0)
                        throw new System.DataMisalignedException("0 quantity, cannot sell this position");
                    return Random.Next(1, position.CurrentQuantity);
                case TradeType.Buy:
                    return Random.Next(1, 300);
                default:
                    throw new System.Exception("invalid trade type");
            }

        }
    }
}