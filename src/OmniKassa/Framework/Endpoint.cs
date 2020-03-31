#if NET452

using OmniKassa.Exceptions;
using OmniKassa.Model;
using OmniKassa.Model.Order;
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