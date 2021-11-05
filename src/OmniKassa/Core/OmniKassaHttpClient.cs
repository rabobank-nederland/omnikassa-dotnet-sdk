#if NETCOREAPP3_1  || NET5_0_OR_GREATER

using System;
using System.Net.Http;
using System.Threading.Tasks;
using OmniKassa.Model;
using OmniKassa.Model.Order;
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

        }

        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL
        /// </summary>
        /// <param name="order">Merchant order</param>
        /// <param name="token">Access token</param>
        /// <returns>Response with payment URL</returns>
        public Task<MerchantOrderResponse> AnnounceMerchantOrder(MerchantOrder order, String token)
        {
            DateTime now = DateTime.Now;
            order.Timestamp = now.ToString("s") + now.ToString("zzz");
            return PostAsync<MerchantOrderResponse>(mClient, PATH_ANNOUNCE_ORDER, token, order);
        }

        /// <summary>
        /// Retrieves the order status data from OmniKassa
        /// </summary>
        /// <param name="notification">Notification received from the webhook</param>
        /// <returns>Order status info</returns>
        public Task<MerchantOrderStatusResponse> GetOrderStatusData(ApiNotification notification)
        {
            return GetAsync<MerchantOrderStatusResponse>(mClient,
                                       PATH_GET_ORDER_STATUS + notification.EventName,
                                       notification.Authentication);
        }

        /// <summary>
        /// Retrieves the available payment brands
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>Payment brands</returns>
        public Task<PaymentBrandsResponse> RetrievePaymentBrands(String token)
        {
            return GetAsync<PaymentBrandsResponse>(mClient, PATH_GET_PAYMENT_BRANDS, token);
        }

        /// <summary>
        /// Retrieves the available iDEAL issuers
        /// </summary>
        /// <param name="token">Access token</param>
        /// <returns>iDEAL issuers</returns>
        public Task<IdealIssuersResponse> RetrieveIdealIssuers(String token)
        {
            return GetAsync<IdealIssuersResponse>(mClient, PATH_GET_IDEAL_ISSUERS, token);
        }

        /// <summary>
        /// Retrieves a new token.
        /// </summary>
        /// <returns>New access token</returns>
        /// <param name="refreshToken">Refresh token</param>
        public Task<AccessToken> RetrieveNewToken(String refreshToken)
        {
            return GetAsync<AccessToken>(mClient, PATH_GET_ACCESS_TOKEN, refreshToken);
        }

        private async Task<T> PostAsync<T>(HttpClient client, string path, string token, object input) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, path);
            request.Headers.ExpectContinue = false;
            request.Content = GetHttpContentForPost(input);

            UpdateHttpClientAuth(client, token);

            HttpResponseMessage response = await client.SendAsync(request);
            return await ProcessResponse<T>(response);
        }

        private async Task<T> GetAsync<T>(HttpClient client, string path, string token) where T : class
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, path);
            request.Headers.ExpectContinue = false;

            UpdateHttpClientAuth(client, token);

            HttpResponseMessage response = await client.SendAsync(request);
            return await ProcessResponse<T>(response);
        }

        private async Task<T> ProcessResponse<T>(HttpResponseMessage response) where T : class
        {
            String result = await response.Content.ReadAsStringAsync();
            return ProcessResult<T>(result);
        }
    }
}

#endif