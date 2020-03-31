using OmniKassa;
using OmniKassa.Samples.DotNet461.Models;
using OmniKassa.Samples.DotNet461.Helpers;
using OmniKassa.Exceptions;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;
using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Net;
using OmniKassa.Model.Response.Notification;

namespace OmniKassa.Samples.DotNet461.Controllers
{
    public class HomeController : Controller
    {
        private static readonly string SIGNING_KEY = "J+seXdXvw9SMuZu2sphZjRX+3p1gJfmZMenIjeQy+m0=";
        private static readonly string TOKEN = "eyJraWQiOiIvKzdpVE5PL0FmSEhKN05kYmFWVGcyZTR6eXFjN3dYV3pFT08wcktoU0NJPSIsImFsZyI6IlJTMjU2In0.eyJta2lkIjoyNDI2NSwiZW52IjoiUyIsImV4cCI6NzI1ODAyODQwMH0.KnwaMEQSsAmF6Z1K7O6D8JMAPr6PgI16KYGUMv6jTO7BseX35N8C79jb8TjxVz27pX4d4jba1DpCPRE27zY70Y2OjDZOsiW9LZiTABe69ST8CchfrVXBQFtyiGLJis3L-fC21efntUiEfkM1m83UyDew5TIMADqMJOIV1G_lQ_rrzje5J35aVtLgIBUHi80cGqAyNoqBaZMspUhLK27pdybbIrFAPuS0rd_1K1tUhEB5MVnkF_N6KIlNotAdQdypT3WeUhfJtfEhn4SI9eFZ5dcz1ag6sdh_vZz174qkLOiLDvOOIuSXOXuLiM_dAS1pZepOJiREFqizNDkedpH48Q";

        private static readonly string RETURN_URL = "http://localhost:52000/Home/Callback/";

        private static string SESSION_ORDER = "Order";

        private static Endpoint omniKassa;

        private int orderId = 0;
        private WebShopModel webShopModel;
        private int orderItemId = 0;

        private static ApiNotification notification;

        public HomeController()
        {
            if (omniKassa == null)
            {
                omniKassa = Endpoint.Create(Environment.SANDBOX, SIGNING_KEY, TOKEN);
            }
            webShopModel = SessionVar.Get<WebShopModel>(SESSION_ORDER);
            if(webShopModel != null)
            {
                orderId = webShopModel.OrderId;
                orderItemId = webShopModel.GetLastItemId();
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
            catch(ArgumentException ex)
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
                    }
                    while (response.MoreOrderResultsAvailable);
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

        public MerchantOrder.Builder GetOrder(int orderId)
        {
            CustomerInformation customerInformation = new CustomerInformation.Builder()
                    .WithTelephoneNumber("0204971111")
                    .WithInitials("J.D.")
                    .WithGender(Gender.M)
                    .WithEmailAddress("johndoe@rabobank.com")
                    .WithDateOfBirth("20-03-1987")
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