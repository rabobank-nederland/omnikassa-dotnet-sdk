using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniKassa.Exceptions;
using OmniKassa.Model.Response;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace OmniKassa.Http
{
    /// <summary>
    /// OmniKassa API client functions
    /// </summary>
    public sealed partial class OmniKassaHttpClient : IDisposable
    {
        private static readonly string SUFFIX = "/omnikassa-api/";
        private static readonly string PATH_ANNOUNCE_ORDER = SUFFIX + "order/server/api/v2/order";
        private static readonly string PATH_GET_ORDER_STATUS = SUFFIX + "order/server/api/v2/events/results/";
        private static readonly string PATH_GET_ORDER_BY_ID = "v2/orders/{0}";
        private static readonly string PATH_GET_PAYMENT_BRANDS = SUFFIX + "order/server/api/payment-brands";
        private static readonly string PATH_GET_IDEAL_ISSUERS = SUFFIX + "ideal/server/api/v2/issuers";
        private static readonly string PATH_GET_ACCESS_TOKEN = SUFFIX + "gatekeeper/refresh";
        private static readonly string PATH_POST_REFUND_REQUEST = SUFFIX + "order/server/api/v2/refund/transactions/{0}/refunds";
        private static readonly string PATH_GET_REFUND_REQUEST = SUFFIX + "order/server/api/v2/refund/transactions/{0}/refunds/{1}";
        private static readonly string PATH_GET_REFUNDABLE_DETAILS_REQUEST = SUFFIX + "order/server/api/v2/refund/transactions/{0}/refundable-details";
        private static readonly string PATH_GET_SHOPPER_PAYMENT_DETAILS = "v1/shopper-payment-details";
        private static readonly string PATH_DELETE_SHOPPER_PAYMENT_DETAILS = "v1/shopper-payment-details/{0}";

        private static readonly string HEADER_REFUND_REQUEST_ID = "request-id";
        private static readonly string HEADER_X_API_USER_AGENT = "X-Api-User-Agent";

        private static readonly string SMARTPAY_USER_AGENT = "RabobankOmnikassaDotNetSDK/1.5.0";

        /// <summary>
        /// Signing key
        /// </summary>
        public byte[] SigningKey { get; private set; }
        /// <summary>
        /// User agent
        /// </summary>
        public string UserAgent { get; private set; }
        /// <summary>
        /// Partner reference
        /// </summary>
        public string PartnerReference { get; private set; }

        private readonly HttpClient mClient;

        /// <summary>
        /// Initializes an ApiConnector with given base URL and signing key
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key</param>
        /// <param name="userAgent">User-Agent value you want to give your implementation</param>
        /// <param name="partnerReference">Can be filled with the partner reference, if applicable</param>
        public OmniKassaHttpClient(String baseURL, byte[] signingKey, string userAgent, string partnerReference)
        {
            SigningKey = signingKey;
            UserAgent = userAgent;
            PartnerReference = partnerReference;

            var handler = GetPlatformHttpHandler();
            mClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseURL)
            };
            mClient.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        // Try to obtain a platform-specific HttpMessageHandler from the other partial.
        // The framework partial may implement `CreatePlatformHandler`; otherwise the
        // core default returns null.
        private HttpMessageHandler GetPlatformHttpHandler()
        {
            return CreatePlatformHandler();
        }

#if !NETFRAMEWORK
        // Default implementation for non-framework targets: return default handler.
        private HttpMessageHandler CreatePlatformHandler()
        {
            return new HttpClientHandler();
        }
#endif

        private HttpContent GetHttpContentForPost(object input)
        {
            String value = JsonConvert.SerializeObject(input);
            return new StringContent(value, Encoding.UTF8, "application/json");
        }

        private void UpdateHttpClientAuth(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }

        /// Processes the API response JSON result.
        /// Validates API-level errors and, if the deserialized object implements
        /// <typeparam name="T">Type to deserialize the JSON result to.</typeparam>
        /// <param name="result">JSON result string returned by the API.</param>
        /// <returns>Deserialized object of type <typeparamref name="T"/>.</returns>
        public T ProcessResult<T>(String result)
        {
            CheckForErrorsInResponse(result);

            T data = JsonConvert.DeserializeObject<T>(result);

            if (data is SignedResponse)
            {
                SignedResponse signedResponse = data as SignedResponse;
                signedResponse.ValidateSignature(SigningKey);
            }
            return data;
        }

        private void CheckForErrorsInResponse(String json)
        {
            try
            {
                JObject jsonObject = JObject.Parse(json);

                if (jsonObject.ContainsKey(OmniKassaErrorResponse.ERROR_CODE_FIELD_NAME))
                {
                    throw IllegalApiResponseException.Of(json);
                }
            }
            catch (JsonReaderException)
            {
                // Response body is not valid JSON — ignore and let callers handle it.
            }
        }

        private string GetUserAgentHeaderString()
        {
            string userAgentHeader = SMARTPAY_USER_AGENT;
            if (PartnerReference != null)
            {
                userAgentHeader += " (pr: " + PartnerReference + ")";
            }
            if (UserAgent != null)
            {
                userAgentHeader += " " + UserAgent;
            }
            return userAgentHeader;
        }

        /// <summary>
        /// Disposes the HttpClient.
        /// </summary>
        public void Dispose()
        {
            mClient.Dispose();
        }
    }
}
