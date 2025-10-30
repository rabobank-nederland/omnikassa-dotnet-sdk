using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Requested customer information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CustomerInformationResponse
    {
        /// <summary>
        /// The first name of the shopper (can be different from the first name in the address)
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the shopper (can be different from the last name in the address)
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// The e-mail address of the shopper
        /// </summary>
        [JsonProperty(PropertyName = "emailAddress")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// The phone number of the shopper
        /// </summary>
        [JsonProperty(PropertyName = "telephoneNumber")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// Company information
        /// </summary>
        [JsonProperty(PropertyName = "company")]
        public CompanyInformationResponse Company { get; set; }

        /// <summary>
        /// Initializes an empty RequestedCustomerInformation
        /// </summary>
        public CustomerInformationResponse()
        {
        }
    }
}
