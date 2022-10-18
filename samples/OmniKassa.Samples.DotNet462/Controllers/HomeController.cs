using OmniKassa.Samples.DotNet462.Models;
using OmniKassa.Samples.DotNet462.Helpers;
using OmniKassa.Exceptions;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Mvc;
using System.Net;
using OmniKassa.Model.Response.Notification;
using OmniKassa.Model.Request;
using OmniKassa.Model;

namespace OmniKassa.Samples.DotNet462.Controllers
{
    public class HomeController : Controller
    {
        private readonly string SIGNING_KEY;
        private readonly string TOKEN;
        private readonly string RETURN_URL;
        private readonly string BASE_URL;

        private static string SESSION_ORDER = "Order";

        private static Endpoint omniKassa;

        private int orderId = 0;
        private WebShopModel webShopModel;
        private int orderItemId = 0;

        private static ApiNotification notification;

        public HomeController()
        {
            var appSettings = ConfigurationManager.AppSettings;
            SIGNING_KEY = appSettings["SigningKey"];
            TOKEN = appSettings["RefreshToken"];
            RETURN_URL = appSettings["CallbackUrl"];
            BASE_URL = appSettings["BaseUrl"];
            
            if (omniKassa == null)
            {
                InitializeOmniKassaEndpoint();
            }

            webShopModel = SessionVar.Get<WebShopModel>(SESSION_ORDER);
            if (webShopModel != null)
            {
                orderId = webShopModel.OrderId;
                orderItemId = webShopModel.GetLastItemId();
            }
        }
        private void InitializeOmniKassaEndpoint()
        {
            if (String.IsNullOrEmpty(BASE_URL))
            {
                omniKassa = Endpoint.Create(Environment.SANDBOX, SIGNING_KEY, TOKEN);
            }
            else
            {
                omniKassa = Endpoint.Create(BASE_URL, SIGNING_KEY, TOKEN);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            SetVersionViewData();
            CreateOrderIfRequired();

            return View(webShopModel);
        }

        private void SetVersionViewData()
        {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
        }

        private void CreateOrderIfRequired()
        {
            if (Session[SESSION_ORDER] == null)
            {
                webShopModel = new WebShopModel(GetOrder(++orderId));
                SessionVar.Set<WebShopModel>(SESSION_ORDER, webShopModel);
            }
        }

        [HttpPost]
        public ActionResult AddItem()
        {
            SetVersionViewData();
            CreateOrderIfRequired();

            NameValueCollection collection = Request.Form;
            try
            {
                OrderItem item = OrderHelper.CreateOrderItem(collection, ++orderItemId);
                webShopModel.AddItem(item);
            }
            catch (ArgumentException ex)
            {
                webShopModel.Error = ex.Message;
            }

            return View("Index", webShopModel);
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
            CreateOrderIfRequired();
            webShopModel.PaymentCompleted = null;

            try
            {
                MerchantOrder order = OrderHelper.PrepareOrder(Request.Form, webShopModel);
                if (order != null)
                {
                    MerchantOrderResponse response = omniKassa.Announce(order);

                    webShopModel = null;
                    SessionVar.Set<WebShopModel>(SESSION_ORDER, null);

                    return new RedirectResult(response.RedirectUrl);
                }
            }
            catch (RabobankSdkException ex)
            {
                webShopModel.Error = ex.Message;
            }
            catch (ArgumentException ex)
            {
                webShopModel.Error = ex.Message;
            }

            return View("Index", webShopModel);
        }

        [HttpGet]
        public ActionResult Callback()
        {
            SetVersionViewData();
            CreateOrderIfRequired();

            try
            {
                webShopModel.PaymentCompleted = PaymentCompletedResponse.Create(Request.QueryString, SIGNING_KEY);
            }
            catch (RabobankSdkException ex)
            {
                webShopModel.Error = ex.Message;
            }

            return View("Index", webShopModel);
        }

        [HttpPost]
        public ActionResult Webhook(ApiNotification notification)
        {
            HomeController.notification = notification;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult RetrieveUpdates()
        {
            if (notification != null)
            {
                try
                {
                    MerchantOrderStatusResponse response = null;
                    do
                    {
                        response = omniKassa.RetrieveAnnouncement(notification);
                        webShopModel.Responses.Add(response);
                    } while (response.MoreOrderResultsAvailable);
                }
                catch (RabobankSdkException ex)
                {
                    webShopModel.Error = ex.Message;
                }
            }
            else
            {
                webShopModel.Error = "Order status notification not yet received.";
            }

            return View("Index", webShopModel);
        }

        [HttpPost]
        public ActionResult RetrievePaymentBrands()
        {
            try
            {
                webShopModel.PaymentBrandsResponse = omniKassa.RetrievePaymentBrands();
            }
            catch (RabobankSdkException ex)
            {
                webShopModel.Error = ex.Message;
            }

            return View("Index", webShopModel);
        }

        [HttpPost]
        public ActionResult RetrieveIdealIssuers()
        {
            try
            {
                webShopModel.IdealIssuersResponse = omniKassa.RetrieveIdealIssuers();
            }
            catch (RabobankSdkException ex)
            {
                webShopModel.Error = ex.Message;
            }

            return View("Index", webShopModel);
        }

        [HttpGet]
        public ActionResult InitiateRefund()
        {
            CreateOrderIfRequired();

            NameValueCollection collection = Request.Params;
            var transactionIdStr = collection.Get("transactionId");
            if (!string.IsNullOrEmpty(transactionIdStr))
            {
                var transactionId = Guid.Parse(transactionIdStr);
                webShopModel.TransactionRefundableDetailsResponse = omniKassa.FetchRefundableTransactionDetails(transactionId);
            }

            return View(webShopModel);
        }

        [HttpPost]
        public ActionResult SubmitRefund()
        {
            try
            {
                NameValueCollection collection = Request.Form;
                var amountStr = collection.Get("amount");
                var description = collection.Get("description");
                var vatCategoryStr = collection.Get("vat");
                var transactionId = Guid.Parse(collection.Get("transactionId"));

                decimal amountDecimal = Convert.ToDecimal(amountStr);
                Money amount = Money.FromDecimal(Currency.EUR, Decimal.Round(amountDecimal, 2));
                VatCategory vatCategory = (VatCategory)Enum.Parse(typeof(VatCategory), vatCategoryStr);

                var refundRequest = new InitiateRefundRequest(amount, description, vatCategory);

                webShopModel.RefundDetailsResponse = omniKassa.InitiateRefundTransaction(refundRequest, transactionId, Guid.NewGuid());
            }
            catch (RabobankSdkException ex)
            {
                webShopModel.Error = ex.Message;
            }
            return View("RefundDetails", webShopModel);
        }

        [HttpGet]
        public ActionResult FetchRefundDetails()
        {
            try
            {
                NameValueCollection collection = Request.Form;
                var transactionIdStr = collection.Get("transactionId");
                var refundIdStr = collection.Get("refundId");

                if (!string.IsNullOrEmpty(transactionIdStr) && string.IsNullOrEmpty(refundIdStr))
                {
                    var transactionId = Guid.Parse(transactionIdStr);
                    var refundId = Guid.Parse(refundIdStr);

                    webShopModel.RefundDetailsResponse = omniKassa.FetchRefundTransaction(transactionId, refundId);
                }
            }
            catch (RabobankSdkException ex)
            {
                webShopModel.Error = ex.Message;
            }
            return View("RefundDetails", webShopModel);
        }

        [HttpGet]
        public ActionResult RefundDetails()
        {
            return View(webShopModel);
        }

        public MerchantOrder.Builder GetOrder(int orderId)
        {
            CustomerInformation customerInformation = new CustomerInformation.Builder()
                .WithTelephoneNumber("0204971111")
                .WithInitials("J.D.")
                .WithGender(Gender.M)
                .WithEmailAddress("johndoe@rabobank.com")
                .WithDateOfBirth("20-03-1987")
                .WithFullName("Jan de Ruiter")
                .Build();

            Address shippingDetails = new Address.Builder()
                .WithFirstName("John")
                .WithLastName("Doe")
                .WithStreet("Street")
                .WithHouseNumber("5")
                .WithHouseNumberAddition("a")
                .WithPostalCode("1234AB")
                .WithCity("Haarlem")
                .WithCountryCode(CountryCode.NL)
                .Build();

            Address billingDetails = new Address.Builder()
                .WithFirstName("John")
                .WithLastName("Doe")
                .WithStreet("Factuurstraat")
                .WithHouseNumber("5")
                .WithHouseNumberAddition("a")
                .WithPostalCode("1234AB")
                .WithCity("Haarlem")
                .WithCountryCode(CountryCode.NL)
                .Build();

            MerchantOrder.Builder order = new MerchantOrder.Builder()
                .WithMerchantOrderId(Convert.ToString(orderId))
                .WithDescription("An example description")
                .WithCustomerInformation(customerInformation)
                .WithShippingDetail(shippingDetails)
                .WithBillingDetail(billingDetails)
                .WithLanguage(Language.NL)
                .WithMerchantReturnURL(RETURN_URL)
                .WithInitiatingParty("LIGHTSPEED");

            return order;
        }
    }
}