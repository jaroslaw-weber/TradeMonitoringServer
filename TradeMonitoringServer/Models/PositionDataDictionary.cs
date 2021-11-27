using System;
using System.Collections.Generic;
using System.Linq;

namespace TradeMonitoringServer
{
    public class PositionDataDictionary : Dictionary<int, PositionData>, IClone<PositionDataDictionary>
    {
        public void ApplyTrade(TradeData trade)
        {
            var position = this[trade.PositionId];
            switch (trade.TradeType)
            {
                case TradeType.Buy:
                    position.CurrentQuantity += trade.Quantity;
                    return;
                case TradeType.Sell:
                    position.CurrentQuantity -= trade.Quantity;
                    return;
                default: throw new System.SystemException("invalid trade type");

            }
        }

        public PositionDataDictionary Clone()
        {
            return (PositionDataDictionary)this.ToDictionary(x => x.Key, y => y.Value.Clone());
        }
    }
}