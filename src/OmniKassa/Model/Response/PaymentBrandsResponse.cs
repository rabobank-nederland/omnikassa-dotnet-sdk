using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Holder object for response retrieving a list of payment brands
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentBrandsResponse
    {
        /// <summary>
        /// List of one or more order results
        /// </summary>
        [JsonProperty(PropertyName = "paymentBrands")]
        public List<PaymentBrandInfo> PaymentBrands { get; private set; }

        /// <summary>
        /// Initializes an empty PaymentBrandsResponse
        /// </summary>
        public PaymentBrandsResponse()
        {

        }
    }
}
