using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Address information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AddressInformationResponse
    {
        /// <summary>
        /// The first name of the shopper
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The middle name of the shopper.
        /// </summary>
        public string MiddleName { get; set; }
        
        /// <summary>
        /// The last name of the shopper
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The street of the address
        /// </summary>
        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; }

        /// <summary>
        /// The house number of the address
        /// </summary>
        [JsonProperty(PropertyName = "houseNumber")]
        public string HouseNumber { get; set; }

        /// <summary>
        /// The addition of the address
        /// </summary>
        [JsonProperty(PropertyName = "houseNumberAddition")]
        public string HouseNumberAddition { get; set; }

        /// <summary>
        /// The postal code
        /// </summary>
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// The city of the address
        /// </summary>
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        /// <summary>
        /// The country code (ISO 3166-1 alpha-2)
        /// </summary>
        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Initializes an empty RequestedAddressInformation
        /// </summary>
        public AddressInformationResponse()
        {
        }
    }
}
