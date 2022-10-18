using Newtonsoft.Json;
using OmniKassa.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Model.Request
{
    /// <summary>
    /// class for initiating refund
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InitiateRefundRequest
    {
        /// <summary>
        /// Amount of money that must be refunded
        /// </summary>
        [JsonProperty(PropertyName = "money")]
        public Money Money { get; private set; }

        /// <summary>
        /// Description of the refund
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public String Description { get; private set; }

        /// <summary>
        /// VAT category
        /// </summary>
        [JsonProperty(PropertyName = "vatCategory")]
        [JsonConverter(typeof(VatCategoryJsonConverter))]
        public VatCategory? VatCategory { get; private set; }

        /// <summary>
        /// Creates an InitiateRefundRequest
        /// </summary>
        /// <param name="money">Refund amount</param>
        /// <param name="description">Refund description</param>
        /// <param name="vatCategory">Refund VAT category</param>
        public InitiateRefundRequest(Money money, String description, VatCategory? vatCategory)
        {
            Money = money;
            Description = description;
            VatCategory = vatCategory;
        }
    }
}
