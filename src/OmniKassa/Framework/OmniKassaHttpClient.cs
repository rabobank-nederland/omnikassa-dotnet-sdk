#if NET462

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OmniKassa.Exceptions;
using OmniKassa.Model;
using OmniKassa.Model.Order;
using OmniKassa.Model.Request;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;

namespace OmniKassa.Http
{
    /// <summary>
    /// OmniKassa API client functions
    /// </summary>
    public sealed partial class OmniKassaHttpClient
    {
        private void InitCertificate()
        {
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Certificate validation callback.
        /// </summary>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (error == System.Net.Security.SslPolicyErrors.None)
            {
                return true;
            }

            throw new RabobankSdkException(String.Format("X509Certificate [{0}] Policy Error: '{1}'", cert.Subject, error.ToString()));
        }

        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL
        /// </summary>
        /// <param name="order">Merchant order</param>
        /// <param name="token">Access token</param>
        /// <returns>Response with payment URL</returns>
        public MerchantOrderResponse AnnounceMerchantOrder(MerchantOrder order, String token)
        {
            DateTime now = DateTime.Now;
            order.Timestamp = now.ToString("s") + now.ToString("zzz");
            var headers = new Dictionary<string, string>()
            {
                { HEADER_X_API_USER_AGENT, GetUserAgentHeaderString() }
            };
            return PostAsync<MerchantOrderResponse>(mClient, PATH_ANNOUNCE_ORDER, headers, token, order);
        }

        /// <summary>
        /// Retrieves the order status data from OmniKassa
        /// </summary>
        /// <param name="notification">Notification received from the webhook</param>
        /// <returns>Order status info</returns>
        public MerchantOrderStatusResponse GetOrderStatusData(ApiNotification apiNotification)
        {
            return GetAsync<MerchantOrderStatusResponse>(mClient,
                                       PATH_GET_ORDER_STATUS + apiNotification.EventName,
                                       apiNotification.Authentication);
        }

        /// <summary>
        /// sends the InitiateRefundRequest to the Rabobank.
        /// </summary>
        /// <param name="refundRequest">containing detail of the refund</param>
        /// <param name="transactionId">id of transaction</param>
        /// <param name="requestId">id of request</param>
        /// <param name="token">Access token</param>
        /// <returns>RefundDetailsResponse for requested refund/returns>
        public RefundDetailsResponse PostRefundRequest(InitiateRefundRequest refundRequest, Guid transactionId, Guid requestId, String token)
        {
            string path = string.Format(PATH_POST_REFUND_REQUEST, transactionId);
            var headers = new Dictionary<string, string>()
            {
                { HEADER_REFUND_REQUEST_ID, requestId.ToString() }
            };
            return PostAsync<RefundDetailsResponse>(mClient, path, headers, token, refundRequest);
        }
        
        /// <summary>
        /// retrieves the RefundDetailsResponse from the Rabobank.
        /// </summary>
        /// <param name="transactionId">id of transaction</param>
        /// <param name="refundId">id of the refund</param>
        /// <param name="token">Access token</param>
        /// <returns>RefundDetailsResponse for requested refund</returns>
        public RefundDetailsResponse GetRefundRequest(Guid transactionId, Guid refundId, String token)
        {
            string path = string.Format(PATH_GET_REFUND_REQUEST, transactionId, refundId);
            return GetAsync<RefundDetailsResponse>(mClient, path, token);
        }
        
        /// <summary>
        /// retrieves the TransactionRefundableDetailsResponse from the Rabobank.
        /// </summary>
        /// <param name="transactionId">id of transaction</param>
        /// <param name="token">access token</param>
        /// <returns>TransactionRefundableDetailsResponse for initiated refund</returns>
        public TransactionRefundableDetailsResponse GetRefundableDetails(Guid transactionId, String token)
        {
            string path = string.Format(PATH_GET_REFUNDABLE_DETAILS_REQUEST, transactionId);
            return GetAsync<TransactionRefundableDetailsResponse>(mClient, path, token);
        }

        /// <summary>
        /// Retrieves the available payment brands
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>Payment brands</returns>
        public PaymentBrandsResponse RetrievePaymentBrands(String token)
        {
            return GetAsync<PaymentBrandsResponse>(mClient, PATH_GET_PAYMENT_BRANDS, token);
        }

        /// <summary>
        /// Retrieves the available iDEAL issuers
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>iDEAL issuers</returns>
        public IdealIssuersResponse RetrieveIdealIssuers(String token)
        {
            return GetAsync<IdealIssuersResponse>(mClient, PATH_GET_IDEAL_ISSUERS, token);
        }

        /// <summary>
        /// Retrieves a new token.
        /// </summary>
        /// <returns>New access token</returns>
        /// <param name="refreshToken">Refresh token</param>
        public AccessToken RetrieveNewToken(String refreshToken)
        {
            return GetAsync<AccessToken>(mClient, PATH_GET_ACCESS_TOKEN, refreshToken);
        }

        private T PostAsync<T>(HttpClient client, string path, Dictionary<string, string> headers, string token, object input) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.ExpectContinue = false;
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
            }
            request.Content = GetHttpContentForPost(input);

            UpdateHttpClientAuth(client, token);

            Task<HttpResponseMessage> response = client.SendAsync(request);
            response.Wait();

            return ProcessResponse<T>(response.Result);
        }

        private T GetAsync<T>(HttpClient client, string path, string token) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.ExpectContinue = false;

            UpdateHttpClientAuth(client, token);

            Task<HttpResponseMessage> response = client.SendAsync(request);
            response.Wait();

            return ProcessResponse<T>(response.Result);
        }

        private T ProcessResponse<T>(HttpResponseMessage response) where T : class
        {
            Task<String> result = response.Content.ReadAsStringAsync();
            return ProcessResult<T>(result.Result);
        }
    }
}

#endif