using System;
using OmniKassa.Http;
using OmniKassa.Model.Response;

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
        /// <param name="environment">Environment to use</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboar</param>
        /// <param name="token">Refresh token from the OmniKassa Dashboard</param>
        /// <param name="userAgent">User-Agent value you want to give your implementation</param>
        /// <returns></returns>
        public static Endpoint Create(
            Environment environment, 
            String signingKey, 
            String token,
            String userAgent
        ) {
            TokenProvider tokenProvider = new InMemoryTokenProvider(token);
            String baseUrl = EnvironmentHelper.GetUrl(environment);
            return Create(baseUrl, signingKey, tokenProvider, userAgent, null);
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="environment">Environment to use</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboar</param>
        /// <param name="token">Refresh token from the OmniKassa Dashboard</param>
        /// <param name="userAgent">User-Agent value you want to give your implementation</param>
        /// <param name="partnerReference">Can be filled with the partner reference, if applicable</param>
        /// <returns></returns>
        public static Endpoint Create(
            Environment environment,
            String signingKey,
            String token,
            String userAgent,
            String partnerReference
        ) {
            TokenProvider tokenProvider = new InMemoryTokenProvider(token);
            String baseUrl = EnvironmentHelper.GetUrl(environment);
            return Create(baseUrl, signingKey, tokenProvider, userAgent, partnerReference);
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboard</param>
        /// <param name="token">Refresh token from the OmniKassa Dashboard</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(
            String baseURL,
            String signingKey,
            String token
        ) {
            if (Enum.TryParse(baseURL, out Environment environment))
            {
                return Create(environment, signingKey, token, null, null);
            }
            TokenProvider tokenProvider = new InMemoryTokenProvider(token);
            return Create(baseURL, signingKey, tokenProvider, null, null);
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboard</param>
        /// <param name="token">Refresh token from the OmniKassa Dashboard</param>
        /// <param name="userAgent">User-Agent value you want to give your implementation</param>
        /// <param name="partnerReference">Can be filled with the partner reference, if applicable</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(
            String baseURL, 
            String signingKey, 
            String token, 
            String userAgent,
            String partnerReference
        ) {
            if (Enum.TryParse(baseURL, out Environment environment))
            {
                return Create(environment, signingKey, token, userAgent, partnerReference);
            }
            TokenProvider tokenProvider = new InMemoryTokenProvider(token);
            return Create(baseURL, signingKey, tokenProvider, userAgent, partnerReference);
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
            return Create(baseURL, signingKeyBytes, tokenProvider, null, null);
        }

        /// <summary>
        /// Creates an instance of OmniKassa
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key from the OmniKassa Dashboard</param>
        /// <param name="tokenProvider">Token provider storing token info</param>
        /// <param name="userAgent">User-Agent value you want to give your implementation</param>
        /// <param name="partnerReference">Can be filled with the partner reference, if applicable</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(
            String baseURL, 
            String signingKey, 
            TokenProvider tokenProvider,
            String userAgent,
            String partnerReference
        ) {
            byte[] signingKeyBytes = Convert.FromBase64String(signingKey);
            return Create(baseURL, signingKeyBytes, tokenProvider, userAgent, partnerReference);
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
        /// <param name="userAgent">User-Agent value you want to give your implementation</param>
        /// <param name="partnerReference">Can be filled with the partner reference, if applicable</param>
        /// <returns>OmniKassa instance</returns>
        public static Endpoint Create(
            String baseURL, 
            byte[] signingKey, 
            TokenProvider tokenProvider, 
            String userAgent,
            String partnerReference
        ) {
            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(baseURL, signingKey, userAgent, partnerReference);
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
