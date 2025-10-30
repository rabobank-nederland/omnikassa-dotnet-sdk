using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;

namespace example_dotnet60.Models
{
    [JsonObject]
    public class WebShopModel
    {
        public MerchantOrder.Builder MerchantOrderBuilder { get; set; }

        public MerchantOrder Order { get; set; }
        
        public int OrderId { get; set; }
        
        public string MerchantReturnUrl { get; set; }

        public PaymentCompletedResponse PaymentCompleted { get; set; }
        
        public List<MerchantOrderStatusResponse> Responses { get; set; } = new List<MerchantOrderStatusResponse>();

        public OrderStatusResult OrderStatusResult { get; set; }

        public PaymentBrandsResponse PaymentBrandsResponse { get; set; }

        public IdealIssuersResponse IdealIssuersResponse { get; set; }

        public RefundDetailsResponse RefundDetailsResponse { get; set; }
        
        public TransactionRefundableDetailsResponse TransactionRefundableDetailsResponse { get; set; }

        public List<CardOnFile> CardsOnFile { get; set; } = new List<CardOnFile>();

        public string ShopperReference { get; set; } = "";

        public string Error { get; set; }

        public string OmniKassaOrderId { get; set; } = "";

        public WebShopModel()
        {

        }

        public WebShopModel(MerchantOrder.Builder order)
        {
            AssignOrder(order);
        }

        public void AssignOrder(MerchantOrder.Builder order)
        {
            MerchantOrderBuilder = order;
            this.OrderId = Convert.ToInt32(order.MerchantOrderId);
            this.MerchantReturnUrl = order.MerchantReturnURL;
            BuildOrder();
        }

        public void ReCreateBuilder()
        {
            MerchantOrderBuilder = new MerchantOrder.Builder()
                .WithMerchantOrderId(Order.MerchantOrderId)
                .WithMerchantReturnURL(Order.MerchantReturnURL)
                .WithAmount(Order.Amount)
                .WithLanguage(Language.NL)
                .WithDescription(Order.Description)
                .WithShippingDetail(Order.ShippingDetails)
                .WithBillingDetail(Order.BillingDetails)
                .WithCustomerInformation(Order.CustomerInformation)
                .WithPaymentBrand(Order.PaymentBrand)
                .WithPaymentBrandForce(Order.PaymentBrandForce)
                .WithPaymentBrandMetaData(Order.PaymentBrandMetaDataObject)
                .WithInitiatingParty(Order.InitiatingParty)
                .WithSkipHppResultPage(Order.SkipHppResultPage)
                .WithShopperBankstatementReference(Order.ShopperBankstatementReference)
                .WithOrderItems(new List<OrderItem>(Order.OrderItems))
                .WithPaymentBrandMetaData(Order.PaymentBrandMetaData)
                .WithShopperReference(Order.ShopperReference)
                .WithShippingCost(Order.ShippingCost);

            if (Order.Language != null)
            {
                MerchantOrderBuilder.WithLanguage((Language)Order.Language);
            }
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

        public MerchantOrder PrepareMerchantOrder(
            Decimal totalPrice,
            CustomerInformation customerInformation,
            Address shippingDetails,
            Address billingDetails,
            PaymentBrand? paymentBrand,
            PaymentBrandForce? paymentBrandForce,
            PaymentBrandMetaData? paymentBrandMetaData,
            string initiatingParty,
            bool skipHppResultPage,
            string shopperBankstatementReference,
            bool enableCardOnFile,
            string shopperReference,
            Decimal? shippingCostAmount,
            Currency? shippingCostCurrency
        ) {
            var builder = MerchantOrderBuilder
                .WithAmount(Money.FromDecimal(Currency.EUR, totalPrice))
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
                .WithShopperBankstatementReference(shopperBankstatementReference)
                .WithShopperReference(shopperReference)
                .WithShippingCost(shippingCostCurrency, shippingCostAmount);

            return builder.Build();
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

        public List<CardOnFile> GetCardsOnFile()
        {
            if (CardsOnFile != null)
            {
                return CardsOnFile;
            }
            return new List<CardOnFile>();
        }

        public Decimal GetShippingCostAmount()
        {
            return MerchantOrderBuilder.ShippingCost.Amount; 
        }

        public Currency GetShippingCostCurrency()
        {
            return MerchantOrderBuilder.ShippingCost.Currency;
        }
    }
}
