using System;

namespace TradeMonitoringServer
{
    public class PositionData : IClone<PositionData>
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string? Ticker { get; set; }

        /// <summary>
        /// How many shares of the position have right now
        /// </summary>
        public int CurrentQuantity { get; set; }

        /// <summary>
        /// How many shares of the position had at the start of the day
        /// </summary>
        public int DayStartQuantity { get; set; }
        /// <summary>
        /// How many shares of the position were sold/bought
        /// </summary>
        public int QuantityTraded { get; set; }

        public PositionData Clone()
        {
            return (PositionData)this.MemberwiseClone();
        }
    }
}
