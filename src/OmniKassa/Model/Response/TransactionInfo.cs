using Newtonsoft.Json;
using OmniKassa.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Contains details about a transaction, these details can be used to identify specific transaction within the order.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TransactionInfo
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; private set; }

        /// <summary>
        /// Payment method
        /// </summary>
        [JsonProperty(PropertyName = "paymentBrand")]
        [JsonConverter(typeof(EnumJsonConverter<PaymentBrand>))]
        public PaymentBrand? PaymentBrand { get; private set; }

        /// <summary>
        /// Type of transaction
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [JsonConverter(typeof(EnumJsonConverter<TransactionType>))]
        public TransactionType? Type { get; private set; }

        /// <summary>
        /// Transaction status
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        [JsonConverter(typeof(EnumJsonConverter<TransactionStatus>))]
        public TransactionStatus? Status { get; private set; }

        /// <summary>
        /// Amount that must be paid
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public Money Amount { get; private set; }

        /// <summary>
        /// Amount that has been paid
        /// </summary>
        [JsonProperty(PropertyName = "confirmedAmount")]
        public Money ConfirmedAmount { get; private set; }

        /// <summary>
        /// Time at creation
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public string StartTime { get; private set; }

        /// <summary>
        /// Last updated time
        /// </summary>
        [JsonProperty(PropertyName = "lastUpdateTime")]
        public string LastUpdateTime { get; private set; }

        /// <summary>
        /// Gets the signature data
        /// </summary>
        /// <returns>Signature data</returns>
        public List<string> GetSignatureData()
        {
            return new List<string>(new string[] {
                Id.ToString(),
                PaymentBrand?.ToString(),
                Type?.ToString(),
                Status?.ToString(),
                Amount.Currency.ToString(),
                Convert.ToString(Amount.GetAmountInCents()),
                ConfirmedAmount?.Currency.ToString(),
                ConfirmedAmount != null ? Convert.ToString(ConfirmedAmount.GetAmountInCents()) : null,
                StartTime,
                LastUpdateTime
            });
        }
    }
}
