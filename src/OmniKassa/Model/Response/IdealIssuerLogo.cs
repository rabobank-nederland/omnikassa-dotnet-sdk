using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// Contains details about an issuer logo.
    /// This can be used for displaying the logo of a particular issuer.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class IdealIssuerLogo
    {
        /// <summary>
        /// Constructor of IdealIssuerLogo
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mimeType"></param>
        public IdealIssuerLogo(string url, string mimeType)
        {
            this.Url = url;
            this.MimeType = mimeType;
        }

        /// <summary>
        /// A publicly accessible URL where you can download the logo of the issuer.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; private set; }

        /// <summary>
        /// The mime type of the logo. This always an image type.
        /// </summary>
        [JsonProperty(PropertyName = "mimeType")]
        public string MimeType { get; private set; }

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
            if (!(obj is IdealIssuerLogo))
            {
                return false;
            }
            IdealIssuerLogo logo = (IdealIssuerLogo)obj;
            return Equals(Url, logo.Url) &&
                Equals(MimeType, logo.MimeType);
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
                hash = (hash * -1521134295) + (Url == null ? 0 : Url.GetHashCode());
                hash = (hash * -1521134295) + (MimeType == null ? 0 : MimeType.GetHashCode());
                return hash;
            }
        }
    }
}