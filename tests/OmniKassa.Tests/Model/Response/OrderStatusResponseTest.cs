using OmniKassa.Model.Response;
using OmniKassa.Tests;
using Xunit;

namespace omnikassa_dotnet_test.Model.Response
{
    public class OrderStatusResponseTest
    {
        [Fact]
        public void JsonShouldBeDeserializedPropery()
        {
            var expected = TestHelper.GetObjectFromJsonFile<OrderStatusResponse>("order-detail.json");
            
            Assert.Equal("1d0a95f4-2589-439b-9562-c50aa19f9caf", expected.Order.Id);
            Assert.Equal(10997, expected.Order.TotalAmount.GetAmountInCents());
            Assert.Single(expected.Order.Transactions);
        }
    }
}