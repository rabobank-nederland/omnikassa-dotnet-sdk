using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Orders;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.RaboOmniKassa.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using OmniKassa;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;

namespace Nop.Plugin.Payments.RaboOmniKassa
{
    /// <summary>
    /// Rabo OmniKassa payment processor.
    /// </summary>
    public class RaboOmniKassaPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RaboOmniKassaPaymentManager _raboOmniKassaPaymentManager;
        private readonly RaboOmniKassaPaymentSettings _raboOmniKassaPaymentSettings;

        #endregion Fields

        #region Constructor

        public RaboOmniKassaPaymentProcessor(CurrencySettings currencySettings,
            ILocalizationService localizationService,
            IOrderTotalCalculationService orderTotalCalculationService,
            ISettingService settingService,
            IWebHelper webHelper,
            IHttpContextAccessor httpContextAccessor,
            RaboOmniKassaPaymentManager raboOmniKassaPaymentManager,
            RaboOmniKassaPaymentSettings raboOmniKassaPaymentSettings)
        {
            _currencySettings = currencySettings;
            _localizationService = localizationService;
            _orderTotalCalculationService = orderTotalCalculationService;
            _settingService = settingService;
            _webHelper = webHelper;
            _httpContextAccessor = httpContextAccessor;
            _raboOmniKassaPaymentManager = raboOmniKassaPaymentManager;
            _raboOmniKassaPaymentSettings = raboOmniKassaPaymentSettings;
        }

        #endregion Constructor

        #region Utilities

        /// <summary>
        /// Get the merchant return URL.
        /// </summary>
        private string GetReturnUrl()
        {
            return _webHelper.GetStoreLocation() + "Plugins/PaymentRaboOmniKassa/ReturnUrl/";
        }

        #endregion Utilities

        #region Methods

        /// <summary>
        /// Process a payment.
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult();
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            var order = postProcessPaymentRequest.Order;
            var customer = order.Customer;
            

            var rokOrderItems = new List<OmniKassa.Model.Order.OrderItem>();

            foreach (var orderItem in order.OrderItems)
            {
                decimal amount = Decimal.Round(orderItem.UnitPriceInclTax, 2);
                decimal tax = Decimal.Round(orderItem.UnitPriceInclTax - orderItem.UnitPriceExclTax, 2);

                var rokOrderItem = new OmniKassa.Model.Order.OrderItem.Builder()
                    .WithId(orderItem.OrderItemGuid.ToString())
                    .WithQuantity(orderItem.Quantity)
                    .WithName(orderItem.Product.Name)
                    .WithDescription(orderItem.Product.ShortDescription)
                    .WithAmount(Money.FromDecimal(OmniKassa.Model.Enums.Currency.EUR, amount))
                    .WithTax(Money.FromDecimal(OmniKassa.Model.Enums.Currency.EUR, tax))
                    .WithItemCategory(ItemCategory.PHYSICAL)
                    .WithVatCategory(VatCategory.LOW)
                    .Build();

                rokOrderItems.Add(rokOrderItem);
            }

            CustomerInformation rokCustomerInformation = new CustomerInformation.Builder()
                    .WithTelephoneNumber(customer.BillingAddress.PhoneNumber)
                    .WithEmailAddress(customer.Email)
                    .Build();

            Address rokShippingDetails = new Address.Builder()
                    .WithFirstName(customer.ShippingAddress.FirstName)
                    .WithLastName(customer.ShippingAddress.LastName)
                    .WithStreet(customer.ShippingAddress.Address1)
                    .WithHouseNumber(customer.ShippingAddress.Address2)
                    .WithPostalCode(customer.ShippingAddress.ZipPostalCode)
                    .WithCity(customer.ShippingAddress.City)
                    .WithCountryCode(CountryCode.NL)
                    .Build();

            Address rokBillingDetails = new Address.Builder()
                    .WithFirstName(customer.BillingAddress.FirstName)
                    .WithLastName(customer.BillingAddress.LastName)
                    .WithStreet(customer.BillingAddress.Address1)
                    .WithHouseNumber(customer.BillingAddress.Address2)
                    .WithPostalCode(customer.BillingAddress.ZipPostalCode)
                    .WithCity(customer.BillingAddress.City)
                    .WithCountryCode(CountryCode.NL)
                    .Build();

            var rokOrder = new MerchantOrder.Builder()
                    .WithMerchantOrderId(order.CustomOrderNumber)
                    .WithDescription(order.CustomOrderNumber)
                    .WithOrderItems(rokOrderItems)
                    .WithAmount(Money.FromDecimal(OmniKassa.Model.Enums.Currency.EUR, Decimal.Round(order.OrderTotal, 2)))
                    .WithCustomerInformation(rokCustomerInformation)
                    .WithShippingDetail(rokShippingDetails)
                    .WithBillingDetail(rokBillingDetails)
                    .WithLanguage(Language.NL)
                    .WithMerchantReturnURL(GetReturnUrl())
                    .Build();

            String redirectUrl = _raboOmniKassaPaymentManager.AnnounceMerchantOrder(rokOrder);

            Debug.WriteLine(redirectUrl);

            _httpContextAccessor.HttpContext.Response.Redirect(redirectUrl);
        }

        /// <summary>
        /// Returns a value indicating whether payment method should be hidden during checkout
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>true - hide; false - display.</returns>
        public bool HidePaymentMethod(IList<ShoppingCartItem> cart)
        {
            //you can put any logic here
            //for example, hide this payment method if all products in the cart are downloadable
            //or hide this payment method if current customer is from certain country
            return false;
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            var result = this.CalculateAdditionalFee(_orderTotalCalculationService, cart,
                _raboOmniKassaPaymentSettings.AdditionalFee, _raboOmniKassaPaymentSettings.AdditionalFeePercentage);

            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //let's ensure that at least 5 seconds passed after order is placed
            //P.S. there's no any particular reason for that. we just do it
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalSeconds < 5)
                return false;

            return true;
        }

        /// <summary>
        /// Validate payment form
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>List of validating errors</returns>
        public IList<string> ValidatePaymentForm(IFormCollection form)
        {
            return new List<string>();
        }

        /// <summary>
        /// Get payment information
        /// </summary>
        /// <param name="form">The parsed form values</param>
        /// <returns>Payment info holder</returns>
        public ProcessPaymentRequest GetPaymentInfo(IFormCollection form)
        {
            return new ProcessPaymentRequest();
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentRaboOmniKassa/Configure";
        }

        /// <summary>
        /// Gets a view component for displaying plugin in public store ("payment info" checkout step)
        /// </summary>
        /// <param name="viewComponentName">View component name</param>
        public void GetPublicViewComponent(out string viewComponentName)
        {
            viewComponentName = "PaymentRaboOmniKassa";
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            return new CapturePaymentResult { Errors = new[] { "Capture method not supported" } };
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            return new RefundPaymentResult { Errors = new[] { "Refund method not supported" } };
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            return new VoidPaymentResult { Errors = new[] { "Void method not supported" } };
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            return new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } };
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            // Settings.
            _settingService.SaveSetting(new RaboOmniKassaPaymentSettings
            {
                UseSandbox = true
            });

            // Locales.
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFee", "Additional fee");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFee.Hint", "Enter additional fee to charge your customers.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFeePercentage", "Additional fee. Use percentage");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFeePercentage.Hint", "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.RedirectionTip", "You will be redirected to Rabo OmniKassa site to complete the order.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.RefreshToken", "Refresh Token");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.RefreshToken.Hint", "Refresh Token Hint");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.SigningKey", "Signing Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.SigningKey.Hint", "Signing Key Hint");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.UseSandbox", "Use Sandbox");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.UseSandbox.Hint", "Check to enable Sandbox (testing environment).");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Instructions", "<p><b>If you're using this gateway ensure that your primary store currency is supported by Rabo OmniKassa 2.0.</b><br /><br />To configure the plugin follow these steps:<br />1. Log into your Rabo OmniKassa 2.0 Dashboard (click <a href=\"https://bankieren.rabobank.nl/omnikassa-dashboard\" target =\"_blank\">here</a> ).<br />2. Copy the Signing Key and Token to the corresponding fields below.<br />3. Enter the URL {0} as the webhook URL.<br /></p>");

            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.RaboOmniKassa.PaymentMethodDescription", "You will be redirected to Rabo OmniKassa site to complete the payment.");

            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            // Settings.
            _settingService.DeleteSetting<RaboOmniKassaPaymentSettings>();

            // Locales.
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFee");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFee.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFeePercentage");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFeePercentage.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.RedirectionTip");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.RefreshToken");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.RefreshToken.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.SigningKey");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.SigningKey.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.UseSandbox");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Fields.UseSandbox.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.Instructions");
            this.DeletePluginLocaleResource("Plugins.Payments.RaboOmniKassa.PaymentMethodDescription");

            base.Uninstall();
        }

        #endregion Methods

        #region Properties

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get { return RecurringPaymentType.NotSupported; }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get { return PaymentMethodType.Redirection; }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a payment method description that will be displayed on checkout pages in the public store
        /// </summary>
        public string PaymentMethodDescription
        {
            //return description of this payment method to be display on "payment method" checkout step. good practice is to make it localizable
            //for example, for a redirection payment method, description may be like this: "You will be redirected to Rabo OmniKassa site to complete the payment"
            get { return _localizationService.GetResource("Plugins.Payments.RaboOmniKassa.PaymentMethodDescription"); }
        }

        #endregion Properties
    }
}