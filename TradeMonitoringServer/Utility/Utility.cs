using System;
namespace TradeMonitoringServer
{
    public static class Utility
    {
        /// <summary>
        /// Global random number generator
        /// </summary>
        public static Random Random { get; private set; } = new Random();
        
    }
}
