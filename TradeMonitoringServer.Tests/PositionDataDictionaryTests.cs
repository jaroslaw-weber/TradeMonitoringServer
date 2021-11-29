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
            //create dummy positions
            var dict = new PositionDataDictionary();
            var position = PositionData.CreateDummy(1);
            dict[position.Id] = position;
            var positionClone = position.Clone();

            //create dummy trade
            var trade = TradeData.CreateDummy(1, position);

            //apply
            dict.ApplyTrade(trade);

            //test if changed correctly
            Assert.NotEqual(position.CurrentQuantity, positionClone.CurrentQuantity);
            Assert.NotEqual(position.QuantityTraded, positionClone.QuantityTraded);
            Assert.Equal(position.DayStartQuantity, positionClone.DayStartQuantity);
        }
    }
}
