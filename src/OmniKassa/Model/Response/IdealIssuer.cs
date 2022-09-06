using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Contains details about an issuer.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class IdealIssuer
    {
        /// <summary>
        /// Constructor of IdealIssuer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="logos"></param>
        /// <param name="countryNames"></param>
        public IdealIssuer(string id, string name, List<IdealIssuerLogo> logos, string countryNames)
        {
            this.Id = id;
            this.Name = name;
            this.Logos = logos;
            this.CountryNames = countryNames;
        }

        /// <summary>
        /// The id of the issuer, which is meant for machines.
        /// Use this if you want to communicate this particular issuer with an API endpoint.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        /// <summary>
        /// The name of the issuer in human readable form.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Can potentially contain multiple formats of the same logo.
        /// </summary>
        [JsonProperty(PropertyName = "logos")]
        public List<IdealIssuerLogo> Logos { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "countryNames")]
        public string CountryNames { get; private set; }

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
            if (!(obj is IdealIssuer))
            {
                return false;
            }
            IdealIssuer issuer = (IdealIssuer)obj;
            return Equals(Id, issuer.Id) &&
                Equals(Name, issuer.Name) &&
                Enumerable.SequenceEqual(Logos, issuer.Logos) &&
                Equals(CountryNames, issuer.CountryNames);
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
                hash = (hash * -1521134295) + Id.GetHashCode();
                hash = (hash * -1521134295) + (Name == null ? 0 : Name.GetHashCode());
                foreach (IdealIssuerLogo result in Logos)
                {
                    hash = (hash * -1521134295) + result.GetHashCode();
                }
                hash = (hash * -1521134295) + (CountryNames == null ? 0 : CountryNames.GetHashCode());
                return hash;
            }
        }
    }
}