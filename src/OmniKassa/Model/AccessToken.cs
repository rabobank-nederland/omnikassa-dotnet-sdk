using System;
using System.Globalization;
using Newtonsoft.Json;
using OmniKassa.Utils;

namespace OmniKassa.Model
{
    /// <summary>
    /// Access token for API access
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AccessToken
    {
        private static readonly double ACCESS_TOKEN_EXPIRATION_MARGIN = 0.01;

        /// <summary>
        /// Access token
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public String Token { get; private set; }

        /// <summary>
        /// Token validity
        /// </summary>
        [JsonProperty(PropertyName = "validUntil")]
        public DateTime? ValidUntil { get; private set; }

        /// <summary>
        /// Token duration validity
        /// </summary>
        [JsonProperty(PropertyName = "durationInMillis")]
        public int DurationInMillis { get; private set; }

        /// <summary>
        /// Initializes a new AccessToken with given parameters
        /// </summary>
        /// <param name="token">Access token</param>
        /// <param name="validUntil">Token validity</param>
        /// <param name="durationInMillis">Token duration validity</param>
        public AccessToken(String token, DateTime? validUntil, int durationInMillis)
        {
            ValidateArguments(token, validUntil, durationInMillis);
            Token = token;
            ValidUntil = validUntil;
            DurationInMillis = durationInMillis;
        }

        private void ValidateArguments(String token, DateTime? validUntil, int durationInMillis)
        {
            if (String.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be empty");
            }
            if (validUntil == null)
            {
                throw new ArgumentException("Valid until cannot be empty");
            }
            if (durationInMillis == 0)
            {
                throw new ArgumentException("Duration in milliseconds cannot be zero");
            }
        }

        /// <summary>
        /// Whether or not the token is expired
        /// </summary>
        /// <returns>true when the token is not yet expired</returns>
        public Boolean IsNotExpired()
        {
            return !IsExpired();
        }

        /// <summary>
        /// Whether or not the token is expired
        /// </summary>
        /// <returns>true when is token is expired</returns>
        public Boolean IsExpired()
        {
            double timeLeft = ((DateTime)ValidUntil - DateTime.Now).TotalMilliseconds;
            return (timeLeft / DurationInMillis) < ACCESS_TOKEN_EXPIRATION_MARGIN;
        }
    }
}
