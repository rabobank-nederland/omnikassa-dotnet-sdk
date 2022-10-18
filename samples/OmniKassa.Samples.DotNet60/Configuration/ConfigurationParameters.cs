namespace OmniKassa.Samples.DotNet50.Configuration
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

        /// <summary>
        /// The URL that points to the Rabobank API
        /// </summary>
        public string BaseUrl { get; }

        public ConfigurationParameters(string refreshToken, string signingKey, string callbackUrl, string baseUrl)
        {
            RefreshToken = refreshToken;
            SigningKey = signingKey;
            CallbackUrl = callbackUrl;
            BaseUrl = baseUrl;
        }
    }
}
