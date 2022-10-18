#if NETSTANDARD1_3 || NETSTANDARD2_0 || NETSTANDARD2_1 || NET5_0_OR_GREATER

using OmniKassa.Exceptions;
using OmniKassa.Model;
using OmniKassa.Model.Order;
using OmniKassa.Model.Request;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;
using System;
using System.Threading.Tasks;

namespace OmniKassa
{
    public sealed partial class Endpoint
    {
        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL and OmniKassa order ID
        /// </summary>
        /// <param name="merchantOrder">Merchant order</param>
        /// <returns>Payment URL and OmniKassa order ID</returns>
        public Task<MerchantOrderResponse> Announce(MerchantOrder merchantOrder)
        {
            return AnnounceMerchantOrder(merchantOrder);
        }

        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL and OmniKassa order ID
        /// </summary>
        /// <param name="merchantOrder">Merchant order</param>
        /// <returns>Payment URL and OmniKassa order ID</returns>
        public async Task<MerchantOrderResponse> AnnounceMerchantOrder(MerchantOrder merchantOrder)
        {
            await ValidateAccessToken();

            try
            {
                return await httpClient.AnnounceMerchantOrder(merchantOrder, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                await RetrieveNewToken();

                return await httpClient.AnnounceMerchantOrder(merchantOrder, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves the order status data from OmniKassa
        /// </summary>
        /// <param name="notification">Notification received from the webhook</param>
        /// <returns>Order status info</returns>
        public async Task<MerchantOrderStatusResponse> RetrieveAnnouncement(ApiNotification notification)
        {
            notification.ValidateSignature(httpClient.SigningKey);

            MerchantOrderStatusResponse response = await httpClient.GetOrderStatusData(notification);
            return response;
        }

        /// <summary>
        /// This function will initiate refund transaction.
        /// </summary>
        /// <param name="refundRequest">The request for refund, the object can be constructed via Jackson or with the direct constructor</param>
        /// <param name="transactionId">The transactionId of transaction for which the refund request is sent</param>
        /// <param name="requestId">The requestId, unique id of refund request</param>
        /// <returns>The response contains refund details, which can be used to update the refund with the latest status.</returns>
        public async Task<RefundDetailsResponse> InitiateRefundTransaction(InitiateRefundRequest refundRequest, Guid transactionId, Guid requestId)
        {
            try
            {
                return await httpClient.PostRefundRequest(refundRequest, transactionId, requestId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                await RetrieveNewToken();

                return await httpClient.PostRefundRequest(refundRequest, transactionId, requestId, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// This function will get refund details.
        /// </summary>
        /// <param name="transactionId">The transactionId of transaction for which the refund request is sent</param>
        /// <param name="refundId">The id of initiated refund request</param>
        /// <returns>The response contains refund details, which can be used to update the refund with the latest status.</returns>
        public async Task<RefundDetailsResponse> FetchRefundTransaction(Guid transactionId, Guid refundId)
        {
            try
            {
                return await httpClient.GetRefundRequest(transactionId, refundId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                await RetrieveNewToken();

                return await httpClient.GetRefundRequest(transactionId, refundId, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// This function will get details for specific refund within transaction.
        /// </summary>
        /// <param name="transactionId">The transactionId of transaction for which the refund request is sent</param>
        /// <returns>The response contains refund details, which can be used to update the refund with the latest status.</returns>
        public async Task<TransactionRefundableDetailsResponse> FetchRefundableTransactionDetails(Guid transactionId)
        {
            try
            {
                return await httpClient.GetRefundableDetails(transactionId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                await RetrieveNewToken();

                return await httpClient.GetRefundableDetails(transactionId, tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves the available payment brands
        /// </summary>
        /// <returns>Payment brands</returns>
        public async Task<PaymentBrandsResponse> RetrievePaymentBrands()
        {
            await ValidateAccessToken();

            try
            {
                return await httpClient.RetrievePaymentBrands(tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                await RetrieveNewToken();

                return await httpClient.RetrievePaymentBrands(tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves the available iDEAL issuers
        /// </summary>
        /// <returns>iDEAL issuers</returns>
        public async Task<IdealIssuersResponse> RetrieveIdealIssuers()
        {
            await ValidateAccessToken();

            try
            {
                return await httpClient.RetrieveIdealIssuers(tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                // We might have mistakenly assumed the token was still valid
                await RetrieveNewToken();

                return await httpClient.RetrieveIdealIssuers(tokenProvider.GetAccessToken());
            }
        }

        /// <summary>
        /// Retrieves a new access token
        /// </summary>
        public async Task RetrieveNewToken()
        {
            AccessToken retrievedToken = await httpClient.RetrieveNewToken(tokenProvider.GetRefreshToken());
            tokenProvider.SetAccessToken(retrievedToken);
        }

        private async Task ValidateAccessToken()
        {
            if (tokenProvider.HasNoValidAccessToken())
            {
                await RetrieveNewToken();
            }
        }
    }
}

#endif