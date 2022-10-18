using OmniKassa.Model.Request;
using OmniKassa.Tests;
using omnikassa_dotnet_test.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace omnikassa_dotnet_test.Model.Request
{
    public class InitiateRefundRequestTest
    {
        [Fact]
        public void TestDefaultRequest()
        {
            InitiateRefundRequest actual = RefundTestFactory.DefaultInitiateRefundRequest();
            InitiateRefundRequest expected =  TestHelper.GetObjectFromJsonFile<InitiateRefundRequest>("refund_initiate_request.json");

            Assert.Equal(expected.Money, actual.Money);
            Assert.Equal(expected.Description, actual.Description);
            Assert.Equal(expected.VatCategory, actual.VatCategory);
        }
    }
}
