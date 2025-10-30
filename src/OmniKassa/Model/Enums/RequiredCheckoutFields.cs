using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// Enumeration of required checkout fields for fast checkout flow
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequiredCheckoutFields
    {
        /// <summary>
        /// Customer information field
        /// </summary>
        [EnumMember(Value = "CUSTOMER_INFORMATION")]
        CUSTOMER_INFORMATION,

        /// <summary>
        /// Billing address field
        /// </summary>
        [EnumMember(Value = "BILLING_ADDRESS")]
        BILLING_ADDRESS,

        /// <summary>
        /// Shipping address field
        /// </summary>
        [EnumMember(Value = "SHIPPING_ADDRESS")]
        SHIPPING_ADDRESS
    }
}
