using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Details of a refundable transaction
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TransactionRefundableDetailsResponse
    {
        /// <summary>
        /// Transaction ID
        /// </summary>
        [JsonProperty(PropertyName = "transactionId")]
        public Guid TransactionId { get; private set; }

        /// <summary>
        /// Amount of money that must be refunded
        /// </summary>
        [JsonProperty(PropertyName = "refundableMoney")]
        public Money RefundableMoney { get; private set; }

        /// <summary>
        /// When expired, a refund is no longer possible
        /// </summary>
        [JsonProperty(PropertyName = "expiryDatetime")]
        public String ExpiryDatetime { get; private set; }
    }
}
