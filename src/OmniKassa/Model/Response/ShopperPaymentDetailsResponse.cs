using Newtonsoft.Json;
using System.Collections.Generic;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response containing all payment details for a shopper
    /// </summary>
    public class ShopperPaymentDetailsResponse
    {
        /// <summary>
        /// The list of cards for the given shopper
        /// </summary>
        [JsonProperty("cardOnFileList")]
        public List<CardOnFile> CardOnFileList { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopperPaymentDetailsResponse()
        {
            CardOnFileList = new List<CardOnFile>();
        }
    }
}
