using Newtonsoft.Json;
using System.Collections.Generic;
using OmniKassa.Model.Enums;
using System;
using System.Linq;

namespace OmniKassa.Model.Order
{
    /// <summary>
    /// Contains fields related to fast checkout flow. When this is defined, it activates the fast checkout flow.
    /// When fast checkout is defined:
    /// - paymentBrandForce must be specified and set to FORCE_ALWAYS.
    /// - specified paymentBrand must be set to 'IDEAL'
    /// </summary>
    public class FastCheckout
    {
        /// <summary>
        /// This field indicates the merchant's desire to receive checkout details of the shopper in
        /// order status response. According to GDPR, the calling party must explicitly ask for the fields
        /// that contain personal information. The requested data will be provided via GET /v2/orders/{orderId} endpoint.
        /// 
        /// Note: This field is needed only for the fast-checkout flow. At least one value must be defined.
        /// </summary>
        [JsonProperty(PropertyName = "requiredCheckoutFields")]
        public IReadOnlyList<RequiredCheckoutFields> RequiredCheckoutFields { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requiredCheckoutFields">List of required checkout fields</param>
        public FastCheckout(IReadOnlyList<RequiredCheckoutFields> requiredCheckoutFields)
        {
            RequiredCheckoutFields = requiredCheckoutFields;
        }

        /// <summary>
        /// Default constructor for JSON deserialization
        /// </summary>
        public FastCheckout()
        {
        }

        /// <summary>
        /// Returns whether a RequiredCheckoutFields value is present.
        /// </summary>
        /// <param name="requiredCheckoutFields"></param>
        /// <returns></returns>
        public Boolean HasRequiredCheckoutFields(RequiredCheckoutFields requiredCheckoutFields)
        {
            return RequiredCheckoutFields.Contains<RequiredCheckoutFields>(requiredCheckoutFields);
        }
        
        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 0x51ed270b;
                
                foreach (var field in RequiredCheckoutFields)
                {
                    hash = (hash * -1521134295) + field.GetHashCode();
                }
                return hash;
            }
        }
    }
}
