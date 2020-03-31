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
            MerchantOrderStatusResponse response = JsonConvert.DeserializeObject<MerchantOrderStatusResponse>("{'orderResults':[{'poiId':'201','totalAmount':{'amount':'100','currency':'EUR'},'errorCode':'NONE','paidAmount':{'amount':'100','currency':'EUR'},'merchantOrderId':'8','orderStatusDateTime':'2016-08-26T13:04:20.304+02:00','orderStatus':'COMPLETED','omnikassaOrderId':'2dbef5cd-f009-461b-b968-57d87000a5b1'}],'signature':'e2051763e4efd43438f70a9df94b161b9c7253919e12ea64983e8789a13b3d5c','moreOrderResultsAvailable':false}");
            String expectedSignatureData = "false,8,2dbef5cd-f009-461b-b968-57d87000a5b1,201,COMPLETED,2016-08-26T13:04:20.304+02:00,NONE,EUR,100,EUR,100";
            String actualSignatureData = String.Join(",", response.GetSignatureData());
            Assert.Equal(expectedSignatureData, actualSignatureData);
        }
    }
}
