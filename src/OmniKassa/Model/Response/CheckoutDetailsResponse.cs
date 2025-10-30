using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Fast checkout details
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CheckoutDetailsResponse
    {
        /// <summary>
        /// Customer information
        /// </summary>
        [JsonProperty(PropertyName = "customerInformation")]
        public CustomerInformationResponse CustomerInformation { get; set; }

        /// <summary>
        /// Billing address information
        /// </summary>
        [JsonProperty(PropertyName = "billingAddress")]
        public AddressInformationResponse BillingAddress { get; set; }

        /// <summary>
        /// Shipping address information
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddress")]
        public AddressInformationResponse ShippingAddress { get; set; }

        /// <summary>
        /// Initializes an empty CheckoutDetails
        /// </summary>
        public CheckoutDetailsResponse()
        {
        }
    }
}
