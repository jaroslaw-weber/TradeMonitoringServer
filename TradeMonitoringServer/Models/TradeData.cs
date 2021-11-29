using System;
using System.Text.Json;

namespace TradeMonitoringServer
{
    /// <summary>
    /// Data of executed trades
    /// </summary>
    public class TradeData
    {
        public int Id { get; private set; }
        /// <summary>
        /// PositionData.Id
        /// </summary>
        public int PositionId { get; private set; }
        /// <summary>
        /// Cache position ticker
        /// </summary>
        public string Ticker { get; private set; }
        /// <summary>
        /// How many shares is selling/buying
        /// </summary>
        public int Quantity { get; private set; }
        /// <summary>
        /// Type of the trade (sell/buy)
        /// </summary>
        public TradeType TradeType { get; private set; }

        public DateTime Timestamp { get; private set; }


        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// Creates dummy data
        /// </summary>
        public static TradeData CreateDummy(int id, PositionData position)
        {
            var trade = new TradeData();
            trade.Id = id;
            trade.Ticker = position.Ticker;
            trade.PositionId = position.Id;
            trade.TradeType = DummyDataHelper.GetRandomTradeType(position);
            trade.Quantity = DummyDataHelper.GetRandomTradeQuantity(position, trade.TradeType);
            trade.Timestamp = DateTime.Now;
            return trade;
        }
    }

}