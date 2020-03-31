using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniKassa.Exceptions;
using OmniKassa.Model.Response;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OmniKassa.Http
{
    /// <summary>
    /// OmniKassa API client functions
    /// </summary>
    public sealed partial class OmniKassaHttpClient
    {
        private static readonly string PATH_ANNOUNCE_ORDER = "order/server/api/v2/order";
        private static readonly string PATH_GET_ORDER_STATUS = "order/server/api/events/results/";
        private static readonly string PATH_GET_PAYMENT_BRANDS = "order/server/api/payment-brands";
        private static readonly string PATH_GET_ACCESS_TOKEN = "gatekeeper/refresh";

        /// <summary>
        /// Signing key
        /// </summary>
        public byte[] SigningKey { get; private set; }
        private HttpClient mClient;

        /// <summary>
        /// Initializes an ApiConnector with given base URL and signing key
        /// </summary>
        /// <param name="baseURL">Base URL for the API</param>
        /// <param name="signingKey">Signing key</param>
        public OmniKassaHttpClient(String baseURL, byte[] signingKey)
        {
            SigningKey = signingKey;

            mClient = new HttpClient
            {
                BaseAddress = new Uri(baseURL)
            };
            mClient.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            InitCertificate();
        }

        private HttpContent GetHttpContentForPost(object input)
        {
            String value = JsonConvert.SerializeObject(input);
            return new StringContent(value, Encoding.UTF8, "application/json");
        }

        private void UpdateHttpClientAuth(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }

        /// <summary>
        /// Processes the API response JSON result 
        /// </summary>
        /// <typeparam name="T">Object type to deserialize the result to</typeparam>
        /// <param name="result">JSON result</param>
        /// <returns>Deserialized object</returns>
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
            JObject jsonObject = JObject.Parse(json);

            if (jsonObject.ContainsKey(OmniKassaErrorResponse.ERROR_CODE_FIELD_NAME))
            {
                throw IllegalApiResponseException.Of(json);
            }
        }
    }
}
