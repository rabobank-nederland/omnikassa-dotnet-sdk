using Newtonsoft.Json;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Company information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CompanyInformationResponse
    {
        /// <summary>
        /// Company name
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Initializes an empty RequestedCompanyInformation
        /// </summary>
        public CompanyInformationResponse()
        {
        }
    }
}
