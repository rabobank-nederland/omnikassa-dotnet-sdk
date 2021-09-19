using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Response from the Rabobank API when request the list of available issuers.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class IdealIssuersResponse
    {
        /// <summary>
        /// Constructor for IdealIssuersResponse
        /// </summary>
        /// <param name="idealIssuers">List of issuers</param>
        public IdealIssuersResponse(List<IdealIssuer> idealIssuers)
        {
            this.IdealIssuers = idealIssuers;
        }

        /// <summary>
        /// The list of issuers that is currently activated for the webshop.
        /// </summary>
        [JsonProperty(PropertyName = "issuers")]
        public List<IdealIssuer> IdealIssuers { get; private set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is IdealIssuersResponse))
            {
                return false;
            }
            IdealIssuersResponse other = (IdealIssuersResponse)obj;
            return Enumerable.SequenceEqual(IdealIssuers, other.IdealIssuers);
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
                foreach (IdealIssuer result in IdealIssuers)
                {
                    hash = (hash * -1521134295) + result.GetHashCode();
                }
                return hash;
            }
        }
    }
}
