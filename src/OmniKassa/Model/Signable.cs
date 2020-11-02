using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using OmniKassa.Exceptions;
using System.Text;
using Newtonsoft.Json;

namespace OmniKassa.Model
{
    /// <summary>
    /// Enforces an object to contain a signature which can be validated
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class Signable
    {
        /// <summary>
        /// Signature of the request or response
        /// </summary>
        [JsonProperty(PropertyName = "signature")]
        public String Signature { get; set; }

        /// <summary>
        /// Gets the signature data
        /// </summary>
        /// <returns>Signature data</returns>
        public abstract List<String> GetSignatureData();

        /// <summary>
        /// Calculates and stores the signature with given signing key
        /// </summary>
        /// <param name="signingKey">Signing key</param>
        public virtual void CalculateSignature(byte[] signingKey)
        {
            Signature = CalculateSignature(GetSignatureData(), signingKey);
        }

        /// <summary>
        /// Calculates the signature with given list of properties and signing key
        /// </summary>
        /// <param name="parts">List of properties</param>
        /// <param name="signingKey">Signing key</param>
        /// <returns>Signature</returns>
        public static String CalculateSignature(List<String> parts, byte[] signingKey)
        {
            String payload = String.Join(",", parts);
            try
            {
                using (HMACSHA512 hmac = new HMACSHA512(signingKey))
                {
                    hmac.Initialize();
                    byte[] bytes = Encoding.UTF8.GetBytes(payload);
                    byte[] rawHmac = hmac.ComputeHash(bytes);
                    return ByteArrayToString(rawHmac);
                }
            }
            catch (Exception anyException)
            {
                throw new RabobankSdkException(anyException);
            }
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
    }
}
