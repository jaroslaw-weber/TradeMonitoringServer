using Xunit;

namespace TradeMonitoringServer.Tests
{
    public class PositionDataDictionaryTests
    {
        /// <summary>
        /// Apply buy trade to position list and check if applied correctly
        /// </summary>
        [Fact]
        public void ApplyBuyTradeTest()
        {
            //create dummy trade
            var trade = new TradeData();
            trade.Id = 1;
            trade.PositionId = 1;
            trade.Quantity = 100;
            trade.TradeType = TradeType.Buy;

            //create dummy positions
            var dict = new PositionDataDictionary();
            var position = new PositionData();
            position.Id = 1;
            position.CurrentQuantity = 100;
            dict[position.Id] = position;

            //apply
            dict.ApplyTrade(trade);

            //test if changed correctly
            int quantity = dict[position.Id].CurrentQuantity;
            int testQuantity = 200;
            Assert.Equal(quantity, testQuantity);
        }
    }
}
