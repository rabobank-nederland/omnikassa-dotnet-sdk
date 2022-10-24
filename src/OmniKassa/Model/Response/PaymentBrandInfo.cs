using Newtonsoft.Json;
using System;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Result object for retrieving payment brand object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentBrandInfo
    {
        /// <summary>
        /// Payment brand name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public String Name { get; private set; }

#pragma warning disable CS0649 // Assigned by response
        [JsonProperty(PropertyName = "status")]
        private readonly String Status;
#pragma warning disable CS0649 // Assigned by response

        /// <summary>
        /// Whether or not this payment brand is active
        /// </summary>
        public Boolean IsActive { get
            {
                return Status.ToUpper().Equals("ACTIVE");
            }
        }
    }
}
