using System;
using System.Text;
using OmniKassa.Model;
using OmniKassa.Model.Order;
using OmniKassa.Utils;
using Xunit;

namespace OmniKassa.Tests.Model.Request
{
    public class MerchantOrderRequestTest
    {
        private readonly DateTime mDate;

        public MerchantOrderRequestTest()
        {
            mDate = DateTime.Parse("2017-08-07T16:28:51.504+" + TestHelper.GetLocalTimeZone("\\:"));
        }

        [Fact]
        public void TestEquals()
        {
            MerchantOrder merchantOrder1 = MerchantOrderFactory.Any();
            MerchantOrder merchantOrder2 = MerchantOrderFactory.Any();

            Assert.True(merchantOrder1.Equals(merchantOrder2));
            Assert.True(merchantOrder1.GetHashCode() == merchantOrder2.GetHashCode());

            MerchantOrder merchantOrder3 = MerchantOrderFactory.IncludingOptionalFields();
            MerchantOrder merchantOrder4 = MerchantOrderFactory.IncludingOptionalFields();

            Assert.True(merchantOrder3.Equals(merchantOrder4));
            Assert.True(merchantOrder3.GetHashCode() == merchantOrder4.GetHashCode());
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject()
        {
            MerchantOrder expected = TestHelper.GetObjectFromJsonFile<MerchantOrder>("merchant_order_request_simple.json");

            MerchantOrder actual = MerchantOrderFactory.Any();
            actual.Timestamp = "2017-08-07T16:28:51.504+" + TestHelper.GetLocalTimeZone("\\:");

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject_Full()
        {
            MerchantOrder expected = TestHelper.GetObjectFromJsonFile<MerchantOrder>("merchant_order_request_full.json");

            MerchantOrder actual = MerchantOrderFactory.IncludingOptionalFields();
            actual.Timestamp = "2017-08-07T16:28:51.504+" + TestHelper.GetLocalTimeZone("\\:");

            Assert.Equal(expected, actual);
        }
    }
}
