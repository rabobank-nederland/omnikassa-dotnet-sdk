using Newtonsoft.Json;
using OmniKassa.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response from the Rabobank API when a refund is created
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RefundDetailsResponse
    {
        /// <summary>
        /// Unique ID of this refund
        /// </summary>
        [JsonProperty(PropertyName = "refundId")]
        public Guid RefundId { get; private set; }

        /// <summary>
        /// Transaction ID of the refund
        /// </summary>
        [JsonProperty(PropertyName = "refundTransactionId")]
        public Guid? RefundTransactionId { get; private set; }

        /// <summary>
        /// Created date
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public String CreatedAt { get; private set; }

        /// <summary>
        /// Last updated date
        /// </summary>
        [JsonProperty(PropertyName = "updatedAt")]
        public String UpdatedAt { get; private set; }

        /// <summary>
        /// Amount of money that must be refunded
        /// </summary>
        [JsonProperty(PropertyName = "refundMoney")]
        public Money RefundMoney { get; private set; }

        /// <summary>
        /// VAT category
        /// </summary>
        [JsonProperty(PropertyName = "vatCategory")]
        public String VatCategory { get; private set; }

        /// <summary>
        /// Payment method
        /// </summary>
        [JsonProperty(PropertyName = "paymentBrand")]
        [JsonConverter(typeof(EnumJsonConverter<PaymentBrand>))]
        public PaymentBrand? PaymentBrand { get; private set; }

        /// <summary>
        /// Refund status
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(EnumJsonConverter<RefundStatus>))]
        public RefundStatus? Status { get; private set; }

        /// <summary>
        /// The description given in the refund request
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public String Description { get; private set; }

        /// <summary>
        /// Transaction ID given in the request
        /// </summary>
        [JsonProperty(PropertyName = "transactionId")]
        public Guid TransactionId { get; private set; }
    }
}
