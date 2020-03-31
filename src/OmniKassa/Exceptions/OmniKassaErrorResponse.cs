using System;
using Newtonsoft.Json;

namespace OmniKassa.Exceptions
{
    /// <summary>
    /// Data class for storing the error data when the API returned an error response
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OmniKassaErrorResponse
    {
        /// <summary>
        /// The field that is present when an error is present in an API response
        /// </summary>
        public static String ERROR_CODE_FIELD_NAME = "errorCode";

        /// <summary>
        /// Error code
        /// </summary>
        [JsonProperty(PropertyName = "errorCode")]
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonProperty(PropertyName = "errorMessage")]
        public String ErrorMessage { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "consumerMessage")]
        public String ConsumerMessage { get; private set; }
    }
}
