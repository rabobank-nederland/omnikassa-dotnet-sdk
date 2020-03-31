using Nop.Services.Logging;
using OmniKassa;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;

namespace Nop.Plugin.Payments.RaboOmniKassa.Services
{
    /// <summary>
    /// Represents the Rabo OmniKassa payment manager.
    /// </summary>
    public class RaboOmniKassaPaymentManager
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly RaboOmniKassaPaymentSettings _raboOmniKassaPaymentSettings;
        private static Endpoint _omniKassa;

        #endregion Fields

        #region Constructor

        public RaboOmniKassaPaymentManager(ILogger logger,
            RaboOmniKassaPaymentSettings raboOmniKassaPaymentSettings)
        {
            _logger = logger;
            _raboOmniKassaPaymentSettings = raboOmniKassaPaymentSettings;
        }

        #endregion Constructor

        #region Utilities

        /// <summary>
        /// Get the OmniKassa environment.
        /// </summary>
        private Environment GetOmniKassaEnvironment()
        {
            return _raboOmniKassaPaymentSettings.UseSandbox ? Environment.SANDBOX : Environment.PRODUCTION;
        }

        private void CheckOmniKassaInterface()
        {
            if(_omniKassa == null)
            {
                _omniKassa = Endpoint.Create(GetOmniKassaEnvironment(),
                    _raboOmniKassaPaymentSettings.SigningKey,
                    _raboOmniKassaPaymentSettings.RefreshToken);
            }
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Announces the MerchantOrder to OmniKassa and returns the payment URL
        /// </summary>
        /// <param name="merchantOrder">Merchant order</param>
        /// <returns>Payment URL</returns>
        public string AnnounceMerchantOrder(MerchantOrder merchantOrder)
        {
            CheckOmniKassaInterface();

            return _omniKassa.AnnounceMerchantOrder(merchantOrder);
        }

        /// <summary>
        /// Retrieves the order status data from OmniKassa
        /// </summary>
        /// <param name="notification">Notification received from the webhook</param>
        /// <returns>Order status info</returns>
        public MerchantOrderStatusResponse GetOrderStatusData(ApiNotification notification)
        {
            CheckOmniKassaInterface();

            return _omniKassa.RetrieveAnnouncement(notification);
        }

        #endregion Methods
    }
}