#if NET462

using OmniKassa.Exceptions;
using OmniKassa.Model;
using OmniKassa.Model.Order;
using OmniKassa.Model.Request;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;
using System;

namespace OmniKassa
{
    public sealed partial class Endpoint
    {
        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL and OmniKassa order ID
        /// </summary>
        /// <param name="merchantOrder">Merchant order</param>
        /// <returns>Holder object containing payment URL and OmniKassa order ID</returns>
        public MerchantOrderResponse Announce(MerchantOrder merchantOrder)
        {
            ValidateAccessToken();

            try
            {
                return httpClient.AnnounceMerchantOrder(merchantOrder, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                RetrieveNewToken();

                return httpClient.AnnounceMerchantOrder(merchantOrder, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL
        /// </summary>
        /// <param name="merchantOrder">Merchant order</param>
        /// <returns>Payment URL</returns>
        [Obsolete("AnnounceMerchantOrder is deprecated, please use Announce instead.", false)]
        public string AnnounceMerchantOrder(MerchantOrder merchantOrder)
        {
            return Announce(merchantOrder).RedirectUrl;
        }

        /// <summary>
        /// Retrieves the order status data from OmniKassa
        /// </summary>
        /// <param name="notification">Notification received from the webhook</param>
        /// <returns>Order status info</returns>
        public MerchantOrderStatusResponse RetrieveAnnouncement(ApiNotification notification)
        {
            notification.ValidateSignature(httpClient.SigningKey);

            MerchantOrderStatusResponse response = httpClient.GetOrderStatusData(notification);
            return response;
        }

         /// <summary>
        /// This function will initiate refund transaction.
        /// </summary>
        /// <param name="refundRequest">The request for refund, the object can be constructed via Jackson or with the direct constructor</param>
        /// <param name="transactionId">The transactionId of transaction for which the refund request is sent</param>
        /// <param name="requestId">The requestId, unique id of refund request</param>
        /// <returns>The response contains refund details, which can be used to update the refund with the latest status.</returns>
        public RefundDetailsResponse InitiateRefundTransaction(InitiateRefundRequest refundRequest, Guid transactionId, Guid requestId)
        {
            try
            {
                return httpClient.PostRefundRequest(refundRequest, transactionId, requestId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                RetrieveNewToken();

                return httpClient.PostRefundRequest(refundRequest, transactionId, requestId, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// This function will get refund details.
        /// </summary>
        /// <param name="transactionId">The transactionId of transaction for which the refund request is sent</param>
        /// <param name="refundId">The id of initiated refund request</param>
        /// <returns>The response contains refund details, which can be used to update the refund with the latest status.</returns>
        public RefundDetailsResponse FetchRefundTransaction(Guid transactionId, Guid refundId)
        {
            try
            {
                return httpClient.GetRefundRequest(transactionId, refundId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                RetrieveNewToken();

                return httpClient.GetRefundRequest(transactionId, refundId, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// This function will get details for specific refund within transaction.
        /// </summary>
        /// <param name="transactionId">The transactionId of transaction for which the refund request is sent</param>
        /// <returns>The response contains refund details, which can be used to update the refund with the latest status.</returns>
        public TransactionRefundableDetailsResponse FetchRefundableTransactionDetails(Guid transactionId)
        {
            try
            {
                return httpClient.GetRefundableDetails(transactionId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                RetrieveNewToken();

                return httpClient.GetRefundableDetails(transactionId, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves the available payment brands
        /// </summary>
        /// <returns>Payment brands</returns>
        public PaymentBrandsResponse RetrievePaymentBrands()
        {
            ValidateAccessToken();

            try
            {
                return httpClient.RetrievePaymentBrands(tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                RetrieveNewToken();

                return httpClient.RetrievePaymentBrands(tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves the available iDEAL issuers
        /// </summary>
        /// <returns>iDEAL issuers</returns>
        public IdealIssuersResponse RetrieveIdealIssuers()
        {
            ValidateAccessToken();

            try
            {
                return httpClient.RetrieveIdealIssuers(tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                RetrieveNewToken();

                return httpClient.RetrieveIdealIssuers(tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves a new access token
        /// </summary>
        public void RetrieveNewToken()
        {
            AccessToken retrievedToken = httpClient.RetrieveNewToken(tokenProvider.GetRefreshToken());
            tokenProvider.SetAccessToken(retrievedToken);
        }

        private void ValidateAccessToken()
        {
            if (tokenProvider.HasNoValidAccessToken())
            {
                RetrieveNewToken();
            }
        }
    }
}

#endif