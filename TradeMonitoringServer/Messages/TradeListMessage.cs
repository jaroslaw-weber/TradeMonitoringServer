using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


namespace TradeMonitoringServer
{
    /// <summary>
    /// Trade list which will be pushed to client
    /// </summary>
    public class TradeListMessage : IJson
    {

        public DateTime Timestamp { get; private set; }
        public TradeData[]? Trades { get; private set; }

        /// <summary>
        /// How much trades to show (max)
        /// </summary>
        const int tradeCountLimit = 12;

        private int HowManyTradesSend()
        {
            var allTrades = GetAllTrades();
            return Math.Min(allTrades.Count(), tradeCountLimit);
        }
        private IEnumerable<TradeData> GetAllTrades()
        {
            if (TradeSimulation.Instance == null)
                throw new System.NullReferenceException("trade simulation has not started!");
            return TradeSimulation.Instance.Trades;
        }

        /// <summary>
        /// Get trades which will be send to client.
        /// Send only last X number of trades
        /// </summary>
        private IEnumerable<TradeData> GetTradesToSend()
        {
            return GetAllTrades().Reverse().Take(HowManyTradesSend());
        }

        public TradeListMessage()
        {
            Timestamp = DateTime.Now;
            Trades = GetTradesToSend().ToArray();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}