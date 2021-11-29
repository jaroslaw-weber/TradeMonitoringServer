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
            position.ApplyTrade(trade);
        }

        public PositionDataDictionary Clone()
        {
            var d = new PositionDataDictionary();
            foreach(var item in this)
            {
                d[item.Key] = item.Value.Clone();
            }
            return d;
        }
    }
}