using System;

namespace TradeMonitoringServer
{
    public class PositionData : ICloneable
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string? Ticker { get; set; }
        public int CurrentQuantity { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
