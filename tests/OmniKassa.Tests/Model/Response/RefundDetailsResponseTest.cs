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
    public class RefundDetailsResponseTest
    {
        [Fact]
        public void TestDefaultResponse()
        {
            RefundDetailsResponse actual = RefundTestFactory.DefaultRefundDetailsResponse();

            Assert.Equal(Guid.Parse("25da863a-60a5-475d-ae47-c0e4bd1bec31"), actual.RefundId);
            Assert.Equal(Guid.Parse("25da863a-60a5-475d-ae47-c0e4bd1bec32"), actual.RefundTransactionId);
            Assert.Equal("2000-01-01T00:00:00.000-0200", actual.CreatedAt);
            Assert.Equal("2000-01-01T00:00:00.000-0200", actual.UpdatedAt);
            Assert.Equal(Money.FromDecimal(Currency.EUR, 100), actual.RefundMoney);
            Assert.Equal("1", actual.VatCategory);
            Assert.Equal(PaymentBrand.IDEAL, actual.PaymentBrand);
            Assert.Equal(RefundStatus.SUCCEEDED, actual.Status);
            Assert.Equal("test description", actual.Description);
            Assert.Equal(Guid.Parse("25da863a-60a5-475d-ae47-c0e4bd1bec33"), actual.TransactionId);
        }
    }
}
