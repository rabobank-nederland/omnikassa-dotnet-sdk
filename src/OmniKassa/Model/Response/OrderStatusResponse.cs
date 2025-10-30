using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response returned by OmniKassa when a specific order status is requested.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OrderStatusResponse
    {
        /// <summary>
        /// Order details, including the latest status
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        public OrderStatusResult Order { get; set; }

        /// <summary>
        /// Initializes an empty GetOrderStatusResponse
        /// </summary>
        public OrderStatusResponse()
        {
        }
    }
}
