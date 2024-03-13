using OmniKassa.Exceptions;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;
using System;
using System.Configuration;
using System.Web.Mvc;
using System.Net;
using OmniKassa.Model.Response.Notification;
using OmniKassa.Model;
using System.Collections.Generic;

namespace OmniKassa.Samples.DotNet462.Controllers
{
    public class HomeController : Controller
    {
        private readonly string SIGNING_KEY;
        private readonly string TOKEN;
        private readonly string RETURN_URL;
        private readonly string BASE_URL;

        private static Endpoint omniKassa;
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
            return View();
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
            try
            {
                MerchantOrder order = GetOrder();
                MerchantOrderResponse response = omniKassa.Announce(order);
                return new RedirectResult(response.RedirectUrl);
            }
            catch (RabobankSdkException)
            {
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult Callback()
        {
            try
            {
                PaymentCompletedResponse response = PaymentCompletedResponse.Create(Request.QueryString, SIGNING_KEY);
                String validatedOrderId = response.OrderId;
                PaymentStatus? validatedStatus = response.Status;

                ViewData["OrderId"] = response.OrderId;
                ViewData["Status"] = response.Status;
            }
            catch (IllegalSignatureException)
            {

            }
            catch (RabobankSdkException)
            {

            }

            return View();
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
                    }
                    while (response.MoreOrderResultsAvailable);
                }
                catch (RabobankSdkException)
                {

                }
            }

            return View("Index");
        }

        [HttpPost]
        public ActionResult RetrievePaymentBrands()
        {
            try
            {
                PaymentBrandsResponse response = omniKassa.RetrievePaymentBrands();
            }
            catch (RabobankSdkException)
            {

            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult RetrieveIdealIssuers()
        {
            try
            {
                IdealIssuersResponse response = omniKassa.RetrieveIdealIssuers();
            }
            catch (RabobankSdkException)
            {

            }
            return View("Index");
        }

        public MerchantOrder GetOrder()
        {
            OrderItem orderItem = new OrderItem.Builder()
                    .WithId("1")
                    .WithQuantity(1)
                    .WithName("Test product")
                    .WithDescription("Description")
                    .WithAmount(Money.FromDecimal(Currency.EUR, 10m))
                    .WithTax(Money.FromDecimal(Currency.EUR, 1m))
                    .WithItemCategory(ItemCategory.PHYSICAL)
                    .WithVatCategory(VatCategory.LOW)
                    .Build();

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

            MerchantOrder order = new MerchantOrder.Builder()
                    .WithMerchantOrderId("ORDID123")
                    .WithDescription("An example description")
                    .WithOrderItems(new List<OrderItem>(new OrderItem[] { orderItem }))
                    .WithAmount(Money.FromDecimal(Currency.EUR, 99.99m))
                    .WithCustomerInformation(customerInformation)
                    .WithShippingDetail(shippingDetails)
                    .WithBillingDetail(billingDetails)
                    .WithLanguage(Language.NL)
                    .WithMerchantReturnURL(RETURN_URL)
                    .WithPaymentBrand(PaymentBrand.IDEAL)
                    .WithPaymentBrandForce(PaymentBrandForce.FORCE_ONCE)
                    .WithInitiatingParty("LIGHTSPEED")
                    .Build();

            return order;
        }
    }
}