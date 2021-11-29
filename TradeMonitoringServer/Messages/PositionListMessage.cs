using System;
using System.Linq;
using System.Text.Json;


namespace TradeMonitoringServer
{
    /// <summary>
    /// Position list which will be pushed to client
    /// </summary>
    public class PositionListMessage : IJson
    {
        public DateTime Timestamp { get; private set; }
        public PositionData[]? Positions { get; private set; }

        public PositionListMessage()
        {
            Timestamp = DateTime.Now;
            if (TradeSimulation.Instance == null)
                throw new System.NullReferenceException("trade simulation has not started!");
            Positions = TradeSimulation.Instance?.CurrentPositionsState?.Values?.ToArray();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}