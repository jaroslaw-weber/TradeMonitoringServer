using System;

namespace TradeMonitoringServer
{
    public class PositionData : IClone<PositionData>
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string? Ticker { get; set; }
        public int CurrentQuantity { get; set; }
        public int DayStartQuantity { get; set; }

        public PositionData Clone()
        {
            return (PositionData)this.MemberwiseClone();
        }
    }
}
