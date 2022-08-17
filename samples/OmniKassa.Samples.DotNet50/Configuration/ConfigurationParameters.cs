using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using OmniKassa.Exceptions;
using System.Text;
using Newtonsoft.Json;

namespace OmniKassa.Model
{
    /// <summary>
    /// Configuration parameters.
    /// </summary>
    public class ConfigurationParameters
    {
        /// <summary>
        /// The refresh token needed to retrieve a new AccessToken. It is supplied by the Rabobank.
        /// </summary>
        public string RefreshToken { get; }
        
        /// <summary>
        /// The signing key to validate the signature with. This is the key given by the Rabobank to sign all communication
        /// </summary>
        public string SigningKey { get; }
        
        /// <summary>
        /// The callback URL 
        /// </summary>
        public string CallbackUrl { get; }

        public ConfigurationParameters(string refreshToken, string signingKey, string callbackUrl)
        {
            RefreshToken = refreshToken;
            SigningKey = signingKey;
            CallbackUrl = callbackUrl;
        }
    }
}
