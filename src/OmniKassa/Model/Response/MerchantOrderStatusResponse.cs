using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response returned by OmniKassa when the states of the last orders is requested.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantOrderStatusResponse : SignedResponse
    {
        /// <summary>
        /// Whether or not more order results are available
        /// </summary>
        [JsonProperty(PropertyName = "moreOrderResultsAvailable")]
        public Boolean MoreOrderResultsAvailable { get; private set; }

        /// <summary>
        /// List of one or more order results
        /// </summary>
        [JsonProperty(PropertyName = "orderResults")]
        public List<MerchantOrderResult> OrderResults { get; private set; }

        /// <summary>
        /// Initializes an empty OrderStatusResponse
        /// </summary>
        public MerchantOrderStatusResponse()
        {
            
        }

        /// <summary>
        /// Initializes an OrderStatusResponse with given signature
        /// </summary>
        /// <param name="signature"></param>
        public MerchantOrderStatusResponse(String signature) :
            base(signature)
        {
            
        }

        /// <summary>
        /// Gets the signature data
        /// </summary>
        /// <returns>Signature data</returns>
        public override List<String> GetSignatureData()
        {
            List<String> data = new List<String>
            {
                Convert.ToString(MoreOrderResultsAvailable).ToLower()
            };
            foreach (MerchantOrderResult result in OrderResults)
            {
                data.AddRange(result.GetSignatureData());
            }
            return data;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override Boolean Equals(Object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (!(obj is MerchantOrderStatusResponse))
            {
                return false;
            }
            MerchantOrderStatusResponse that = (MerchantOrderStatusResponse)obj;
            return Equals(Signature, that.Signature) &&
                   Equals(MoreOrderResultsAvailable, that.MoreOrderResultsAvailable) &&
                   Enumerable.SequenceEqual(OrderResults, that.OrderResults);
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
                hash = (hash * -1521134295) + (Signature == null ? 0 : Signature.GetHashCode());
                hash = (hash * -1521134295) + MoreOrderResultsAvailable.GetHashCode();
                foreach (MerchantOrderResult result in OrderResults)
                {
                    hash = (hash * -1521134295) + result.GetHashCode();
                }
                return hash;
            }
        }
    }
}
