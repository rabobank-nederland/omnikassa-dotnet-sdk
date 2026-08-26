#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Net.Http;
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
        // No-op placeholder kept for compatibility with older builds.
        // Certificate handling is performed per-HttpClient by CreateHttpHandler.
        private void InitCertificate()
        {
            // Intentionally left blank. Per-client certificate validation is used instead of
            // modifying ServicePointManager.ServerCertificateValidationCallback and SecurityProtocol.
        }

        // Provide a per-client HttpMessageHandler for the framework build. Keep it
        // simple and rely on the default HttpClientHandler certificate validation.
        private HttpMessageHandler CreatePlatformHandler()
        {
            return new HttpClientHandler();
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
        /// <param name="apiNotification">Notification received from the webhook</param>
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

        /// <summary>
        /// Retrieves all payment details for a shopper
        /// </summary>
        /// <param name="shopperRef">The shopper reference</param>
        /// <param name="token">Access token</param>
        /// <returns>Shopper payment details</returns>
        public ShopperPaymentDetailsResponse GetShopperPaymentDetails(String shopperRef, String token)
        {
            var uriBuilder = new UriBuilder(mClient.BaseAddress)
            {
                Path = PATH_GET_SHOPPER_PAYMENT_DETAILS,
                Query = $"shopper-ref={Uri.EscapeDataString(shopperRef)}"
            };
            string path = uriBuilder.Uri.PathAndQuery;
            return GetAsync<ShopperPaymentDetailsResponse>(mClient, path, token);
        }

        /// <summary>
        /// Deletes a specific shopper payment detail
        /// </summary>
        /// <param name="id">The payment detail ID to delete</param>
        /// <param name="shopperRef">The shopper reference</param>
        /// <param name="token">Access token</param>
        public void DeleteShopperPaymentDetail(String id, String shopperRef, String token)
        {
            var uriBuilder = new UriBuilder(mClient.BaseAddress)
            {
                Path = string.Format(PATH_DELETE_SHOPPER_PAYMENT_DETAILS, Uri.EscapeDataString(id)),
                Query = $"shopper-ref={Uri.EscapeDataString(shopperRef)}"
            };
            string path = uriBuilder.Uri.PathAndQuery;
            DeleteAsync(mClient, path, token);
		}

        /// Retrieves the order status by order ID
        /// </summary>
        /// <param name="orderId">Order ID</param>
        /// <param name="token">Access token</param>
        /// <returns>Order status response</returns>
        public OrderStatusResponse GetOrderById(String orderId, String token)
        {
            var uriBuilder = new UriBuilder(mClient.BaseAddress)
            {
                Path = string.Format(PATH_GET_ORDER_BY_ID, Uri.EscapeDataString(orderId))
            };
            string path = uriBuilder.Uri.PathAndQuery;
            return GetAsync<OrderStatusResponse>(mClient, path, token);
        }

        private T PostAsync<T>(HttpClient client, string path, Dictionary<string, string> headers, string token, object input) where T : class
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, path))
            {
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

                try
                {
                    var respMsg = client.SendAsync(request).GetAwaiter().GetResult();
                    using (var resp = respMsg)
                    {
                        return ProcessResponse<T>(resp);
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new RabobankSdkException(ex);
                }
            }
        }

        private T GetAsync<T>(HttpClient client, string path, string token) where T : class
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, path))
            {
                request.Headers.ExpectContinue = false;

                UpdateHttpClientAuth(client, token);

                try
                {
                    var respMsg = client.SendAsync(request).GetAwaiter().GetResult();
                    using (var resp = respMsg)
                    {
                        return ProcessResponse<T>(resp);
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new RabobankSdkException(ex);
                }
            }
        }

        private void DeleteAsync(HttpClient client, string path, string token)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, path))
            {
                request.Headers.ExpectContinue = false;

                UpdateHttpClientAuth(client, token);

                try
                {
                    var respMsg = client.SendAsync(request).GetAwaiter().GetResult();
                    using (var resp = respMsg)
                    {
                        // Read body so we can parse OmniKassa error payloads for non-success
                        string body = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        if (!resp.IsSuccessStatusCode)
                        {
                            try
                            {
                                CheckForErrorsInResponse(body);
                            }
                            catch (IllegalApiResponseException)
                            {
                                throw;
                            }

                            throw new RabobankSdkException($"HTTP {(int)resp.StatusCode} ({resp.StatusCode}). Response: {body}");
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new RabobankSdkException(ex);
                }
            }
        }

        private T ProcessResponse<T>(HttpResponseMessage response) where T : class
        {
            // Read body first so we can parse OmniKassa error payloads even when
            // the HTTP status is non-success.
            string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    // If the body contains OmniKassa error information, this will
                    // throw IllegalApiResponseException which we want to propagate.
                    CheckForErrorsInResponse(body);
                }
                catch (IllegalApiResponseException)
                {
                    throw;
                }

                // Otherwise surface a generic SDK exception with status and body.
                throw new RabobankSdkException($"HTTP {(int)response.StatusCode} ({response.StatusCode}). Response: {body}");
            }

            return ProcessResult<T>(body);
        }
    }
}

#endif
