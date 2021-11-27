using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using TradeMonitoringServer;


namespace TradeMonitoringServer
{
    /// <summary>
    /// Position list which will be pushed to client
    /// </summary>
    public class PositionListMessage : IJson
    {
        public DateTime Timestamp { get; set; }
        public PositionData[] Positions { get; set; }

        public PositionListMessage()
        {
            Timestamp = DateTime.Now;
            Positions = Program.TradeSimulation.CurrentPositionsState.Values.ToArray();
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}