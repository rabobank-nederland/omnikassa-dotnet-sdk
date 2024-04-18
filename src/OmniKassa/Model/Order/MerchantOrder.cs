using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Order
{
    /// <summary>
    /// Merchant order
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantOrder
    {
        /// <summary>
        /// Merchant order ID
        /// </summary>
        [JsonProperty(PropertyName = "merchantOrderId")]
        public String MerchantOrderId { get; private set; }

        /// <summary>
        /// Amount to pay by the consumer
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; private set; }

        /// <summary>
        /// Desired language for the payment page
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        [JsonConverter(typeof(EnumJsonConverter<Language>))]
        public Language? Language { get; private set; }

        /// <summary>
        /// Description of the order
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public String Description { get; private set; }

        /// <summary>
        /// URL where the consumer will redirected when leaving the payment page
        /// </summary>
        [JsonProperty(PropertyName = "merchantReturnURL")]
        public String MerchantReturnURL { get; private set; }

        /// <summary>
        /// Items part of this order
        /// </summary>
        [JsonProperty(PropertyName = "orderItems")]
        public IReadOnlyList<OrderItem> OrderItems { get; private set; }

        /// <summary>
        /// Delivery address
        /// </summary>
        [JsonProperty(PropertyName = "shippingDetail")]
        public Address ShippingDetails { get; private set; }

        /// <summary>
        /// Invoice address
        /// </summary>
        [JsonProperty(PropertyName = "billingDetail")]
        public Address BillingDetails { get; private set; }

        /// <summary>
        /// Limited set of information about the consumer
        /// </summary>
        [JsonProperty(PropertyName = "customerInformation")]
        public CustomerInformation CustomerInformation { get; private set; }

        /// <summary>
        /// Payment method
        /// </summary>
        [JsonProperty(PropertyName = "paymentBrand")]
        [JsonConverter(typeof(EnumJsonConverter<PaymentBrand>))]
        public PaymentBrand? PaymentBrand { get; private set; }

        /// <summary>
        /// Whether or not the payment method is enforced
        /// </summary>
        [JsonProperty(PropertyName = "paymentBrandForce")]
        [JsonConverter(typeof(EnumJsonConverter<PaymentBrandForce>))]
        public PaymentBrandForce? PaymentBrandForce { get; private set; }

        /// <summary>
        /// Extra information about the payment brand
        /// </summary>
        [JsonProperty(PropertyName = "paymentBrandMetaData", NullValueHandling = NullValueHandling.Ignore)]
        public IReadOnlyDictionary<string, string> PaymentBrandMetaData { get; private set; }

        /// <summary>
        /// Skip result page
        /// </summary>
        [JsonProperty(PropertyName = "skipHppResultPage")]
        public bool SkipHppResultPage { get; private set; }

        /// <summary>
        /// Timestamp of the order
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public String Timestamp { get; set; }

        /// <summary>
        /// Unique ID identifying the entity which created the order
        /// </summary>
        [JsonProperty(PropertyName = "initiatingParty")]
        public String InitiatingParty { get; private set; }

        /// <summary>
        /// Shown on the customer's bankstatement reference.
        /// </summary>
        [JsonProperty(PropertyName = "shopperBankstatementReference")]
        public String ShopperBankstatementReference { get; private set; }

        /// <summary>
        /// Initializes an empty MerchantOrder
        /// </summary>
        public MerchantOrder()
        {
            
        }

        /// <summary>
        /// Initializes a MerchantOrder using the Builder
        /// </summary>
        /// <param name="builder">Builder</param>
        private MerchantOrder(Builder builder)
        {
            this.MerchantOrderId = builder.MerchantOrderId;
            this.Amount = builder.Amount;
            this.Language = builder.Language;
            this.Description = builder.Description;
            this.MerchantReturnURL = builder.MerchantReturnURL;
            this.OrderItems = builder.OrderItems.AsReadOnly();
            this.ShippingDetails = builder.ShippingDetails;
            this.BillingDetails = builder.BillingDetails;
            this.CustomerInformation = builder.CustomerInformation;
            this.PaymentBrand = builder.PaymentBrand;
            this.PaymentBrandForce = builder.PaymentBrandForce;
            this.PaymentBrandMetaData = builder.PaymentBrandMetaData;
            this.SkipHppResultPage = builder.SkipHppResultPage;
            this.InitiatingParty = builder.InitiatingParty;
            this.ShopperBankstatementReference = builder.ShopperBankstatementReference;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is MerchantOrder))
            {
                return false;
            }
            MerchantOrder order = (MerchantOrder)obj;
            return Equals(MerchantOrderId, order.MerchantOrderId) &&
                Equals(Amount, order.Amount) &&
                Equals(Language, order.Language) &&
                Equals(Description, order.Description) &&
                Equals(MerchantReturnURL, order.MerchantReturnURL) &&
                Enumerable.SequenceEqual(OrderItems, order.OrderItems) &&
                Equals(ShippingDetails, order.ShippingDetails) &&
                Equals(BillingDetails, order.BillingDetails) &&
                Equals(CustomerInformation, order.CustomerInformation) &&
                Equals(PaymentBrand, order.PaymentBrand) &&
                Equals(PaymentBrandForce, order.PaymentBrandForce) &&
                EqualsPaymentBrandMetaData(PaymentBrandMetaData, order.PaymentBrandMetaData) &&
                Equals(SkipHppResultPage, order.SkipHppResultPage) &&
                Equals(Timestamp, order.Timestamp) &&
                Equals(InitiatingParty, order.InitiatingParty) &&
                Equals(ShopperBankstatementReference, order.ShopperBankstatementReference);
        }

        private bool EqualsPaymentBrandMetaData(
            IReadOnlyDictionary<string, string> one,
            IReadOnlyDictionary<string, string> two)
        {
            // Handle empty dictionaries as null
            var tmpOne = one != null && one.Count > 0 ? one : null;
            var tmpTwo = two != null && two.Count > 0 ? two : null;

            // When both are null, items are the same.
            if (tmpOne == null && tmpTwo == null)
            {
                return true;
            }
            // If only one is null, items are not the same.
            if (tmpOne == null || tmpTwo == null)
            {
                return false;
            }
            // Order both dictionaries and compare their items.
            return tmpOne.OrderBy(kvp => kvp.Key, StringComparer.Ordinal)
                    .SequenceEqual(tmpTwo.OrderBy(kvp => kvp.Key, StringComparer.Ordinal));
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 0x51ed270b;
                hash = (hash * -1521134295) + (MerchantOrderId == null ? 0 : MerchantOrderId.GetHashCode());
                hash = (hash * -1521134295) + (Amount == null ? 0 : Amount.GetHashCode());
                hash = (hash * -1521134295) + (Language == null ? 0 : Language.GetHashCode());
                hash = (hash * -1521134295) + (Description == null ? 0 : Description.GetHashCode());
                hash = (hash * -1521134295) + (MerchantReturnURL == null ? 0 : MerchantReturnURL.GetHashCode());
                foreach (OrderItem result in OrderItems)
                {
                    hash = (hash * -1521134295) + result.GetHashCode();
                }
                hash = (hash * -1521134295) + (ShippingDetails == null ? 0 : ShippingDetails.GetHashCode());
                hash = (hash * -1521134295) + (BillingDetails == null ? 0 : BillingDetails.GetHashCode());
                hash = (hash * -1521134295) + (CustomerInformation == null ? 0 : CustomerInformation.GetHashCode());
                hash = (hash * -1521134295) + (PaymentBrand == null ? 0 : PaymentBrand.GetHashCode());
                hash = (hash * -1521134295) + (PaymentBrandForce == null ? 0 : PaymentBrandForce.GetHashCode());
                hash = GetHashCodePaymentBrandMetaData(hash);
                hash = (hash * -1521134295) + SkipHppResultPage.GetHashCode();
                hash = (hash * -1521134295) + (Timestamp == null ? 0 : Timestamp.GetHashCode());
                hash = (hash * -1521134295) + (InitiatingParty == null ? 0 : InitiatingParty.GetHashCode());
                hash = (hash * -1521134295) + (ShopperBankstatementReference == null ? 0 : ShopperBankstatementReference.GetHashCode());
                return hash;
            }
        }

        private int GetHashCodePaymentBrandMetaData(int hash)
        {
            if (PaymentBrandMetaData != null)
            {
                var orderedMetaData = PaymentBrandMetaData.OrderBy(kvp => kvp.Key, StringComparer.Ordinal);
                foreach (KeyValuePair<string, string> result in orderedMetaData)
                {
                    hash = (hash * -1521134295) + result.Key.GetHashCode();
                    hash = (hash * -1521134295) + result.Value.GetHashCode();
                }
            }
            return hash;
        }

        /// <summary>
        /// MerchantOrder builder
        /// </summary>
        public class Builder
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public String MerchantOrderId { get; private set; }
            public Money Amount { get; private set; }
            public Language? Language { get; private set; }
            public String Description { get; private set; }
            public String MerchantReturnURL { get; private set; }
            public List<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();
            public Address ShippingDetails { get; private set; }
            public Address BillingDetails { get; private set; }
            public CustomerInformation CustomerInformation { get; private set; }
            public PaymentBrand? PaymentBrand { get; private set; }
            public PaymentBrandForce? PaymentBrandForce { get; private set; }
            public Dictionary<string, string> PaymentBrandMetaData { get; private set; } = new Dictionary<string, string>();
            public bool SkipHppResultPage { get; private set; }
            public String InitiatingParty { get; private set; }
            public String ShopperBankstatementReference { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

            /// <summary>
            /// - Must not be null
            /// - if shopperBankstatementReference is supplied:
            ///     Allows all ascii characters up to a length of 255 characters, if the ID contains more than 255 characters, the extra characters are removed after the 255th character.
            /// - else
            ///     Must adhere to the pattern: '[A-Za-z0-9]{1,24}', if the ID contains more than 24 characters, the extra characters are removed after the 24th character.
            /// </summary>
            /// <param name="merchantOrderId">Order ID</param>
            /// <returns>Builder</returns>
            public Builder WithMerchantOrderId(String merchantOrderId)
            {
                this.MerchantOrderId = merchantOrderId;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must not exceed 99.999,99
            /// </summary>
            /// <param name="amount">
            /// Amount to pay by the consumer. Must be equal to the sum of all order items multiplied with the quantity
            /// including VAT. To prevent rounding errors it is advised to calculate the total VAT based on the price per item.
            /// In case this amount does not match the sum of all order items, the;
            /// - order items will be filtered out, and
            /// - AfterPay not possible as payment method
            /// </param>
            /// <returns>Builder</returns>
            public Builder WithAmount(Money amount)
            {
                this.Amount = amount;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must be a valid Language
            /// - ISO 3166-1 alpha-2
            /// - In case of an unknown language, EN will be used
            /// </summary>
            /// <param name="language">Desired language for the payment page</param>
            /// <returns>Builder</returns>
            public Builder WithLanguage(Language language)
            {
                this.Language = language;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must be a valid String
            /// - Maximum length of 35 characters
            /// </summary>
            /// <param name="description">Description of the order</param>
            /// <returns>Builder</returns>
            public Builder WithDescription(String description)
            {
                this.Description = description;
                return this;
            }

            /// <summary>
            /// - Must be the URL to return to after the payment has been completed
            /// - Must not be null
            /// - Must be a valid web address
            /// </summary>
            /// <param name="merchantReturnURL">URL where the consumer will redirected when leaving the payment page</param>
            /// <returns>Builder</returns>
            public Builder WithMerchantReturnURL(String merchantReturnURL)
            {
                this.MerchantReturnURL = merchantReturnURL;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// </summary>
            /// <param name="orderItems">Items part of this order</param>
            /// <returns>Builder</returns>
            public Builder WithOrderItems(List<OrderItem> orderItems)
            {
                this.OrderItems = orderItems;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Maximum of 99 items
            /// </summary>
            /// <param name="shippingDetails">Delivery address</param>
            /// <returns>Builder</returns>
            public Builder WithShippingDetail(Address shippingDetails)
            {
                this.ShippingDetails = shippingDetails;
                return this;
            }

            /// <summary>
            /// - Optional
            /// </summary>
            /// <param name="billingDetails">Invoice address</param>
            /// <returns>Builder</returns>
            public Builder WithBillingDetail(Address billingDetails)
            {
                this.BillingDetails = billingDetails;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - When value <see cref="PaymentBrand.CARDS"/> is supplied, all card payment methods will be available
            /// </summary>
            /// <param name="paymentBrand">Payment method</param>
            /// <returns>Builder</returns>
            public Builder WithPaymentBrand(PaymentBrand? paymentBrand)
            {
                this.PaymentBrand = paymentBrand;
                return this;
            }

            /// <summary>
            /// - Required when payment brand is supplied
            /// </summary>
            /// <param name="paymentBrandForce">Whether or not the payment method is enforced</param>
            /// <returns>Builder</returns>
            public Builder WithPaymentBrandForce(PaymentBrandForce? paymentBrandForce)
            {
                this.PaymentBrandForce = paymentBrandForce;
                return this;
            }

            /// <summary>
            /// Can be used for supplying extra information about the payment brand.
            /// </summary>
            /// <param name="paymentBrandMetaData">Optional</param>
            /// <returns>Builder</returns>
            public Builder WithPaymentBrandMetaData(Dictionary<string, string> paymentBrandMetaData)
            {
                this.PaymentBrandMetaData = paymentBrandMetaData;
                return this;
            }

            /// <summary>
            /// Set to true if you want the customer to be immediately redirected to your webshop after the payment has concluded.
            /// </summary>
            /// <param name="skipHppResultPage">Optional</param>
            /// <returns>Builder</returns>
            public Builder WithSkipHppResultPage(bool skipHppResultPage)
            {
                this.SkipHppResultPage = skipHppResultPage;
                return this;
            }

            /// <summary>
            /// - Optional
            /// </summary>
            /// <param name="customerInformation">Limited set of information about the consumer</param>
            /// <returns>Builder</returns>
            public Builder WithCustomerInformation(CustomerInformation customerInformation)
            {
                this.CustomerInformation = customerInformation;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Field must remain emty unless specified by Rabobank
            /// </summary>
            /// <param name="initiatingParty">Unique ID identifying the entity which created the order</param>
            /// <returns>Builder</returns>
            public Builder WithInitiatingParty(String initiatingParty)
            {
                this.InitiatingParty = initiatingParty;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must adhere to the pattern: '[A-Za-z0-9]{1,24}', if the ID contains more than 24 characters, the extra characters are removed after the 24th character.
            /// </summary>
            /// <param name="shopperBankstatementReference">Shown on the customer's bankstatement reference.</param>
            /// <returns>Builder</returns>
            public Builder WithShopperBankstatementReference(String shopperBankstatementReference)
            {
                this.ShopperBankstatementReference = shopperBankstatementReference;
                return this;
            }

            /// <summary>
            /// Initializes and returns a MerchantOrder
            /// </summary>
            /// <returns>MerchantOrder</returns>
            public MerchantOrder Build()
            {
                return new MerchantOrder(this);
            }
        }
    }
}
