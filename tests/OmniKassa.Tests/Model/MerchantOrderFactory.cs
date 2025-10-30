using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Tests.Model.Order;

namespace OmniKassa.Tests.Model
{
    public class MerchantOrderFactory
    {
        public static MerchantOrder Any()
        {
            return DefaultBuilder().Build();
        }

        public static MerchantOrder IncludingOptionalFields()
        {
            return DefaultBuilder()
                    .WithShippingDetail(AddressFactory.AddressFull())
                    .WithBillingDetail(AddressFactory.AddressFull())
                    .WithOrderItems(OrderItemFactory.ToList(OrderItemFactory.OrderItemFull()))
                    .WithCustomerInformation(CustomerInformationFactory.CustomerInformationFull())
                    .WithPaymentBrand(PaymentBrand.IDEAL)
                    .WithPaymentBrandForce(PaymentBrandForce.FORCE_ALWAYS)
                    .WithPaymentBrandMetaData(new Dictionary<string, Object>{{"issuerId", "RABONL2U"}})
                    .WithPaymentBrandFastCheckout(new FastCheckout(new List<RequiredCheckoutFields> { RequiredCheckoutFields.BILLING_ADDRESS }))
                    .WithShippingCost(Money.FromDecimal(Currency.EUR, 12.34m))
                    .WithInitiatingParty("LIGHTSPEED")
                    .WithSkipHppResultPage(true)
                    .WithShopperBankstatementReference("bankstatementReference")
                    .Build();
        }

        private static MerchantOrder.Builder DefaultBuilder()
        {
            return new MerchantOrder.Builder()
                    .WithMerchantOrderId("ORDID123")
                    .WithAmount(Money.FromDecimal(Currency.EUR, 4.95m))
                    .WithLanguage(Language.NL)
                    .WithMerchantReturnURL("http://localhost/")
                    .WithDescription("An example description");
        }
    }
}
