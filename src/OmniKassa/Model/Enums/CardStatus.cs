using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// Enumeration of possible card statuses
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardStatus
    {
        /// <summary>
        /// The cars is active
        /// </summary>
        [EnumMember(Value = "ACTIVE")]
        ACTIVE,

        /// <summary>
        /// The card is inactive
        /// </summary>
        [EnumMember(Value = "INACTIVE")]
        INACTIVE,

        /// <summary>
        /// The card is deleted
        /// </summary>
        [EnumMember(Value = "DELETED")]
        DELETED,
        
        /// <summary>
        /// The card is suspended
        /// </summary>
        [EnumMember(Value = "SUSPENDED")]
        SUSPENDED
    }
}