using System;
using System.Collections.Generic;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;

namespace OmniKassa.Samples.DotNet462.Models
{
    public class WebShopModel
    {
        private MerchantOrder.Builder MerchantOrderBuilder;

        public MerchantOrder Order { get; private set; }
        public int OrderId { get; private set; }
        public string MerchantReturnUrl { get; private set; }

        public PaymentCompletedResponse PaymentCompleted { get; set; }
        public List<MerchantOrderStatusResponse> Responses { get; private set; } = new List<MerchantOrderStatusResponse>();

        public PaymentBrandsResponse PaymentBrandsResponse { get; set; }

        public IdealIssuersResponse IdealIssuersResponse { get; set; }

        public RefundDetailsResponse RefundDetailsResponse { get; set; }
        public TransactionRefundableDetailsResponse TransactionRefundableDetailsResponse { get; set; }

        public string Error { get; set; }

        public WebShopModel(MerchantOrder.Builder order)
        {
            MerchantOrderBuilder = order;
            this.OrderId = Convert.ToInt32(order.MerchantOrderId);
            this.MerchantReturnUrl = order.MerchantReturnURL;
            BuildOrder();
        }

        public WebShopModel(int orderId, string merchantReturnUrl)
        {
            this.OrderId = orderId;
            this.MerchantReturnUrl = merchantReturnUrl;
            MerchantOrderBuilder = new MerchantOrder.Builder()
                    .WithMerchantOrderId(Convert.ToString(OrderId))
                    .WithMerchantReturnURL(MerchantReturnUrl);
            BuildOrder();
        }

        public void AddItem(OrderItem item)
        {
            MerchantOrderBuilder.OrderItems.Add(item);
            BuildOrder();
        }

        public void Clear()
        {
            MerchantOrderBuilder.OrderItems.Clear();
            BuildOrder();
        }

        public MerchantOrder BuildOrder()
        {
            Order = MerchantOrderBuilder.Build();
            return Order;
        }

        public MerchantOrder PrepareMerchantOrder(Decimal totalPrice,
                                                  CustomerInformation customerInformation,
                                                  Address shippingDetails,
                                                  Address billingDetails,
                                                  PaymentBrand? paymentBrand,
                                                  PaymentBrandForce? paymentBrandForce,
                                                  Dictionary<string, string> paymentBrandMetaData,
                                                  string initiatingParty,
                                                  bool skipHppResultPage)
        {
            MerchantOrderBuilder.WithAmount(Money.FromDecimal(Currency.EUR, totalPrice))
                    .WithLanguage(Language.NL)
                    .WithDescription("An example description")
                    .WithShippingDetail(shippingDetails)
                    .WithBillingDetail(billingDetails)
                    .WithCustomerInformation(customerInformation)
                    .WithPaymentBrand(paymentBrand)
                    .WithPaymentBrandForce(paymentBrandForce)
                    .WithPaymentBrandMetaData(paymentBrandMetaData)
                    .WithInitiatingParty(initiatingParty)
                    .WithSkipHppResultPage(skipHppResultPage)
                    .Build();

            return MerchantOrderBuilder.Build();
        }

        public Decimal GetTotalPrice()
        {
            Decimal sum = 0.0m;
            foreach (OrderItem item in MerchantOrderBuilder.OrderItems)
            {
                Decimal itemPrice = item.Amount.Amount;
                sum += itemPrice * item.Quantity;
            }
            return sum;
        }

        public int GetLastItemId()
        {
            List<OrderItem> items = MerchantOrderBuilder.OrderItems;
            if (items.Count > 0)
            {
                return Convert.ToInt32(items[items.Count - 1].Id);
            }
            return 0;
        }

        public List<IdealIssuer> GetIdealIssuers()
        {
            if (IdealIssuersResponse != null && IdealIssuersResponse.IdealIssuers != null)
            {
                return IdealIssuersResponse.IdealIssuers;
            }
            return new List<IdealIssuer>();
        }
    }
}
