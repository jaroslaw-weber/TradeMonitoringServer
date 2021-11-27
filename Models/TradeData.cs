using System.Text.Json;

namespace TradeMonitoringServer
{
    /// <summary>
    /// Data of executed trades
    /// </summary>
    public class TradeData
    {
        public int Id { get; set; }
        /// <summary>
        /// PositionData.Id
        /// </summary>
        public int PositionId { get; set; }
        /// <summary>
        /// How much is selling/buying
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Type of the trade (sell/buy)
        /// </summary>
        public TradeType TradeType { get; set; }


        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }

}