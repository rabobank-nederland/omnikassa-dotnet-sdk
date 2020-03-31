using System;
using System.Collections.Generic;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;

namespace OmniKassa.Tests.Model.Order
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
                    .WithOrderItems(OrderItemFactory.ToList((OrderItemFactory.OrderItemFull())))
                    .WithCustomerInformation(CustomerInformationFactory.CustomerInformationFull())
                    .WithPaymentBrand(PaymentBrand.IDEAL)
                    .WithPaymentBrandForce(PaymentBrandForce.FORCE_ALWAYS)
                    .Build();
        }

        public static MerchantOrder WithNegativeItem()
        {
            return DefaultBuilder()
                    .WithShippingDetail(AddressFactory.AddressFull())
                    .WithBillingDetail(AddressFactory.AddressFull())
                    .WithOrderItems(new List<OrderItem>() { OrderItemFactory.OrderItemFull(), OrderItemFactory.OrderItemNegative() })
                    .WithCustomerInformation(CustomerInformationFactory.CustomerInformationFull())
                    .WithPaymentBrand(PaymentBrand.IDEAL)
                    .WithPaymentBrandForce(PaymentBrandForce.FORCE_ALWAYS)
                    .Build();
        }

        public static MerchantOrder.Builder DefaultBuilder()
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
