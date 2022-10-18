using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Response;
using omnikassa_dotnet_test.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace omnikassa_dotnet_test.Model.Response
{
    public class TransactionRefundableDetailsResponseTest
    {
        [Fact]
        public void TestDefaultResponse()
        {
            TransactionRefundableDetailsResponse actual = RefundTestFactory.DefaultTransactionRefundableDetailsResponse();

            Assert.Equal(Guid.Parse("25da863a-60a5-475d-ae47-c0e4bd1bec31"), actual.TransactionId);
            Assert.Equal(Money.FromDecimal(Currency.EUR, 100), actual.RefundableMoney);
            Assert.Equal("2000-01-01T00:00:00.000-0200", actual.ExpiryDatetime);
        }
    }
}
