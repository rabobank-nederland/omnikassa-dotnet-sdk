using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Request;
using OmniKassa.Model.Response;
using OmniKassa.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace omnikassa_dotnet_test.Model.Utils
{
    public class RefundTestFactory
    {
        public static RefundDetailsResponse DefaultRefundDetailsResponse()
        {
            return TestHelper.GetObjectFromJsonFile<RefundDetailsResponse>("refund_details.json");
        }

        public static TransactionRefundableDetailsResponse DefaultTransactionRefundableDetailsResponse()
        {
            return TestHelper.GetObjectFromJsonFile<TransactionRefundableDetailsResponse>("refund_transaction_details.json");
        }

        public static InitiateRefundRequest DefaultInitiateRefundRequest()
        {
            return new InitiateRefundRequest(Money.FromDecimal(Currency.EUR, 100), "description", VatCategory.HIGH);
        }
    }
}
