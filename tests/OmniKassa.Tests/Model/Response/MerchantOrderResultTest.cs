using System;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Response;
using Xunit;

namespace OmniKassa.Tests.Model.Response
{
    public class MerchantOrderResultTest
    {
        [Fact]
        public void TestEquals()
        {
            MerchantOrderResult orderResult1 = GetMerchantOrderResult();
            MerchantOrderResult orderResult2 = GetMerchantOrderResult();

            Assert.True(orderResult1.Equals(orderResult2));
            Assert.True(orderResult1.GetHashCode() == orderResult2.GetHashCode());
        }

        [Fact]
        public void GetSignatureData_Should_ReturnCorrectSignatureData()
        {
            String expectedSignatureData = "SHOP1,ORDER1,1,COMPLETED,2000-01-01T00:00:00.000-0200,NONE,EUR,100,EUR,100";

            MerchantOrderResult actual = GetMerchantOrderResult();
            String actualSignatureData = String.Join(",", actual.GetSignatureData());

            Assert.Equal(expectedSignatureData, actualSignatureData);
        }

        private MerchantOrderResult GetMerchantOrderResult()
        {
            String json = "{ " +
                "poiId: 1, " +
                "merchantOrderId: 'SHOP1', " +
                "omnikassaOrderId: 'ORDER1', " +
                "orderStatus: 'COMPLETED', " +
                "errorCode: 'NONE', " +
                "orderStatusDateTime: '2000-01-01T00:00:00.000-0200', " +
                "paidAmount: " + GetJsonMoney(Currency.EUR, 100) + ", " +
                "totalAmount: " + GetJsonMoney(Currency.EUR, 100) +
                "}";

            return JsonConvert.DeserializeObject<MerchantOrderResult>(json);
        }

        private String GetJsonMoney(Currency currency, int amount)
        {
            return "{ currency: '" + Convert.ToString(currency) + "', amount: '" + amount + "' }";
        }
    }
}
