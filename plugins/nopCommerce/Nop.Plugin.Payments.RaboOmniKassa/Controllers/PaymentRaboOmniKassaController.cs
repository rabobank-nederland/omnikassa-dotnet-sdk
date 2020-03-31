using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Plugin.Payments.RaboOmniKassa.Models;
using Nop.Plugin.Payments.RaboOmniKassa.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using OmniKassa.Exceptions;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;

namespace Nop.Plugin.Payments.RaboOmniKassa.Controllers
{
    public class PaymentRaboOmniKassaController : BasePaymentController
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILocalizationService _localizationService;
        private readonly RaboOmniKassaPaymentManager _raboOmniKassaPaymentManager;
        private readonly RaboOmniKassaPaymentSettings _raboOmniKassaPaymentSettings;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PaymentRaboOmniKassaController(
            ILogger logger,
            IWorkContext workContext,
            IStoreService storeService, 
            ISettingService settingService,
            IPermissionService permissionService,
            IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            ILocalizationService localizationService,
            RaboOmniKassaPaymentManager raboOmniKassaPaymentManager,
            RaboOmniKassaPaymentSettings raboOmniKassaPaymentSettings)
        {
            _logger = logger;
            _workContext = workContext;
            _storeService = storeService;
            _settingService = settingService;
            _permissionService = permissionService;
            _orderService = orderService;
            _orderProcessingService = orderProcessingService;
            _localizationService = localizationService;
            _raboOmniKassaPaymentManager = raboOmniKassaPaymentManager;
            _raboOmniKassaPaymentSettings = raboOmniKassaPaymentSettings;
        }

        #endregion Contructor

        #region Utilities

        private Dictionary<String, String> GetDictionary(Dictionary<String, StringValues> collection)
        {
            var dictionary = new Dictionary<String, String>();
            foreach (var pair in collection)
            {
                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }

        #endregion Utilities

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();
            
            // Load settings for a chosen store scope.
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var raboOmniKassaPaymentSettings = _settingService.LoadSetting<RaboOmniKassaPaymentSettings>(storeScope);

            var model = new ConfigurationModel
            {
                UseSandbox = raboOmniKassaPaymentSettings.UseSandbox,
                SigningKey = raboOmniKassaPaymentSettings.SigningKey,
                RefreshToken = raboOmniKassaPaymentSettings.RefreshToken,
                AdditionalFee = raboOmniKassaPaymentSettings.AdditionalFee,
                AdditionalFeePercentage = raboOmniKassaPaymentSettings.AdditionalFeePercentage,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.UseSandbox_OverrideForStore = _settingService.SettingExists(raboOmniKassaPaymentSettings, x => x.UseSandbox, storeScope);
                model.SigningKey_OverrideForStore = _settingService.SettingExists(raboOmniKassaPaymentSettings, x => x.SigningKey, storeScope);
                model.RefreshToken_OverrideForStore = _settingService.SettingExists(raboOmniKassaPaymentSettings, x => x.RefreshToken, storeScope);
                model.AdditionalFee_OverrideForStore = _settingService.SettingExists(raboOmniKassaPaymentSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = _settingService.SettingExists(raboOmniKassaPaymentSettings, x => x.AdditionalFeePercentage, storeScope);
            }

            return View("~/Plugins/Payments.RaboOmniKassa/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [AdminAntiForgery]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            // Load settings for a chosen store scope.
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var raboOmniKassaPaymentSettings = _settingService.LoadSetting<RaboOmniKassaPaymentSettings>(storeScope);

            // Save settings.
            raboOmniKassaPaymentSettings.UseSandbox = model.UseSandbox;
            raboOmniKassaPaymentSettings.SigningKey = model.SigningKey;
            raboOmniKassaPaymentSettings.RefreshToken = model.RefreshToken;
            raboOmniKassaPaymentSettings.AdditionalFee = model.AdditionalFee;
            raboOmniKassaPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;

            // We do not clear cache after each setting update.
            // This behavior can increase performance because cached settings will 
            // not be cleared and loaded from database after each update.
            _settingService.SaveSettingOverridablePerStore(raboOmniKassaPaymentSettings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(raboOmniKassaPaymentSettings, x => x.SigningKey, model.SigningKey_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(raboOmniKassaPaymentSettings, x => x.RefreshToken, model.RefreshToken_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(raboOmniKassaPaymentSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(raboOmniKassaPaymentSettings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);

            // Now clear the settings cache.
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [HttpPost]
        public IActionResult WebhookEventHandler([FromBody] ApiNotification notification)
        {
            try
            {
                _logger.Information("OmniKassa Webhook called: PoiId=" + notification.PoiId + ", EventName="  + notification.EventName + ", Expiry=" + notification.Expiry);

                MerchantOrderStatusResponse response = null;
                
                do
                {
                    response = _raboOmniKassaPaymentManager.GetOrderStatusData(notification);

                    foreach (var result in response.OrderResults)
                    {
                        _logger.Information("OmniKassa order update: " +
                                "MerchantOrderId=" + result.MerchantOrderId +
                                ", OmnikassaOrderId=" + result.OmnikassaOrderId +
                                ", OrderStatus=" + result.OrderStatus.ToString() +
                                ", Amount=" + result.TotalAmount.Amount.ToString() +
                                ", ErrorCode=" + result.ErrorCode);

                        var order = _orderService.GetOrderById(Convert.ToInt32(result.MerchantOrderId));
                        bool success = HandleOrderStatus(order, result.OrderStatus);
                        if(!success)
                        {
                            _logger.Error($"OmniKassa error: Order with id {result.MerchantOrderId} was not found");
                        }
                    }

                }
                while (response.MoreOrderResultsAvailable);
            }

            catch (RabobankSdkException ex)
            {
                _logger.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            // Nothing should be rendered to visitor
            return Content("");
        }

        /// <summary>
        /// Handler for the Merchant Return Url (specified when announcing an order).
        /// </summary>
        [HttpGet]
        public IActionResult ReturnUrlEventHandler()
        {
            try
            {
                Dictionary<String, StringValues> query = QueryHelpers.ParseQuery(Request.QueryString.Value);
                Dictionary<String, String> dictionary = GetDictionary(query);

                // Parse the response and check if the signature is valid (an exception will be thrown if this is not the case).
                var response = PaymentCompletedResponse.Create(dictionary, _raboOmniKassaPaymentSettings.SigningKey);

                var order = _orderService.GetOrderById(Convert.ToInt32(response.OrderId));
                bool success = HandleOrderStatus(order, response.Status);
                if(success)
                {
                    return RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
                }
                else
                {
                    _logger.Error($"OmniKassa error: Order with id {response.OrderId} was not found");
                }
            }

            catch (RabobankSdkException ex)
            {
                _logger.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private bool HandleOrderStatus(Order order, OmniKassa.Model.Enums.PaymentStatus? status)
        {
            if (order != null)
            {
                switch (status)
                {
                    case OmniKassa.Model.Enums.PaymentStatus.COMPLETED:
                        {
                            if (_orderProcessingService.CanMarkOrderAsPaid(order))
                            {
                                order.CaptureTransactionResult = status.ToString();
                                _orderService.UpdateOrder(order);
                                _orderProcessingService.MarkOrderAsPaid(order);
                            }

                            break;
                        }
                    // TODO: handle other states as required
                    default:
                        {
                            break;
                        }
                }
                return true;
            }
            return false;
        }

        #endregion Methods
    }
}