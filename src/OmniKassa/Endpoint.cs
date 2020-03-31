using System;
using System.Threading.Tasks;
using OmniKassa.Http;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;

namespace OmniKassa
{
    /// <summary>
    /// Entry point for all communication with OmniKassa
    /// </summary>
    public sealed partial class Endpoint
    {
        private OmniKassaHttpClient httpClient;
        private TokenProvider tokenProvider;

        /// <summary>
        /// Signing key to validate all communication
        /// </summary>
        public byte[] SigningKey
        {
            get
            {
                return httpClient.SigningKey;
            }
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="environment">Environment to use</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboar</param>
        /// <param name="token">Refresh token from the OmniKassa Dashboard</param>
        /// <returns></returns>
        public static Endpoint Create(Environment environment, String signingKey, String token)
        {
            TokenProvider tokenProvider = new InMemoryTokenProvider(token);
            String baseUrl = EnvironmentHelper.GetUrl(environment);
            return Create(baseUrl, signingKey, tokenProvider);
        }


        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboard</param>
        /// <param name="token">Refresh token from the OmniKassa Dashboard</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(String baseURL, String signingKey, String token)
        {
            TokenProvider tokenProvider = new InMemoryTokenProvider(token);
            return Create(baseURL, signingKey, tokenProvider);
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboard</param>
        /// <param name="tokenProvider">Token provider storing token info</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(String baseURL, String signingKey, TokenProvider tokenProvider)
        {
            byte[] signingKeyBytes = Convert.FromBase64String(signingKey);
            return Create(baseURL, signingKeyBytes, tokenProvider);
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="tokenProvider">Token provider storing token info</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(OmniKassaHttpClient httpClient, TokenProvider tokenProvider)
        {
            return new Endpoint(httpClient, tokenProvider);
        }

        private Endpoint(OmniKassaHttpClient httpClient, TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboard</param>
        /// <param name="tokenProvider">Token provider storing token info</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(String baseURL, byte[] signingKey, TokenProvider tokenProvider)
        {
            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(baseURL, signingKey);
            return new Endpoint(httpClient, tokenProvider);
        }

        /// <summary>
        /// Validates a SignedResponse object
        /// </summary>
        /// <param name="signedResponse">Signed response</param>
        public void ValidateSignature(SignedResponse signedResponse)
        {
            signedResponse.ValidateSignature(httpClient.SigningKey);
        }
    }
}
