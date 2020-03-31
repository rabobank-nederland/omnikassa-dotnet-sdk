using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response given by the OmniKassa API when an order is announced
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantOrderResponse
    {
        /// <summary>
        /// Url to which the user should be redirected
        /// </summary>
        /// <value>The redirect URL.</value>
        [JsonProperty(PropertyName = "redirectUrl")]
        public String RedirectUrl { get; private set; }

        /// <summary>
        /// Unique order ID that was assigned by OmniKassa
        /// </summary>
        /// <value>The OmniKassa Order ID</value>
        [JsonProperty(PropertyName = "omnikassaOrderId")]
        public String OmnikassaOrderId { get; private set; }

        /// <summary>
        /// Initializes AnnounceOrderResponse
        /// </summary>
        public MerchantOrderResponse()
        {
            
        }
    }
}
