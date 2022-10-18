using System;
using Newtonsoft.Json;
using OmniKassa.Model.Response;
using Xunit;

namespace OmniKassa.Tests.Model.Response
{
    public class MerchantOrderStatusResponseTest
    {
        [Fact]
        public void TestEquals()
        {
            MerchantOrderStatusResponse response1 = new MerchantOrderStatusResponseBuilder().Build();
            MerchantOrderStatusResponse response2 = new MerchantOrderStatusResponseBuilder().Build();

            Assert.True(response1.Equals(response2));
            Assert.True(response1.GetHashCode() == response2.GetHashCode());
        }

        [Fact]
        public void GetSignatureData_Should_ReturnCorrectSignatureData()
        {
            MerchantOrderStatusResponse response = TestHelper.GetObjectFromJsonFile<MerchantOrderStatusResponse>("merchant_order_response_full.json");
            String expectedSignatureData = "false,SHOP1,ORDER1,1,COMPLETED,2000-01-01T00:00:00.000-0200,NONE,EUR,100,EUR,100," +
                                       "2dbef5cd-f009-461b-b968-57d87000a5b2,IDEAL,PAYMENT,SUCCESS,EUR,100,EUR,100,2016-07-28T12:51:15.574+02:00,2016-07-28T12:51:15.574+02:00," +
                                       "2dbef5cd-f009-461b-b968-57d87000a5b3,IDEAL,PAYMENT,SUCCESS,EUR,200,EUR,200,2016-07-28T12:51:15.574+02:00,2016-07-28T12:51:15.574+02:00," +
                                       "2dbef5cd-f009-461b-b968-57d87000a5b4,IDEAL,PAYMENT,SUCCESS,EUR,300,EUR,300,2016-07-28T12:51:15.574+02:00,2016-07-28T12:51:15.574+02:00";
            String actualSignatureData = String.Join(",", response.GetSignatureData());
            Assert.Equal(expectedSignatureData, actualSignatureData);
        }

        [Fact]
        public void GetSignatureDataWithoutTransactions_Should_ReturnCorrectSignatureData()
        {
            MerchantOrderStatusResponse response = TestHelper.GetObjectFromJsonFile<MerchantOrderStatusResponse>("merchant_order_response_simple.json");
            String expectedSignatureData = "false,8,2dbef5cd-f009-461b-b968-57d87000a5b1,201,COMPLETED,2016-08-26T13:04:20.304+02:00,NONE,EUR,100,EUR,100";
            String actualSignatureData = String.Join(",", response.GetSignatureData());
            Assert.Equal(expectedSignatureData, actualSignatureData);
        }
    }
}
