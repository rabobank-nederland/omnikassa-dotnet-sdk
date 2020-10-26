using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OmniKassa;
using System.Collections.Specialized;
using OmniKassa.Model.Response;
using OmniKassa.Exceptions;
using OmniKassa.Model.Enums;
using OmniKassa.Model;
using OmniKassa.Model.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using OmniKassa.Model.Response.Notification;

namespace example_dotnetcore13.Controllers
{
    public class HomeController : Controller
    {
        // Specify your signing key and the refresh token in the static properties below
        private static readonly string SIGNING_KEY = "";
        private static readonly string TOKEN = "";
        private static readonly string RETURN_URL = "http://localhost:52060/Home/Callback/";

        private static Endpoint omniKassa;
        private static ApiNotification notification;

        public HomeController()
        {
            if (omniKassa == null)
            {
                omniKassa = Endpoint.Create(OmniKassa.Environment.SANDBOX, SIGNING_KEY, TOKEN);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PlaceOrder()
        {
            try
            {
                MerchantOrder order = GetOrder();
                MerchantOrderResponse response = await omniKassa.Announce(order);
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
                Dictionary<String, StringValues> query = QueryHelpers.ParseQuery(Request.QueryString.Value);
                Dictionary<String, String> dictionary = GetDictionary(query);

                PaymentCompletedResponse response = PaymentCompletedResponse.Create(dictionary, SIGNING_KEY);
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
        public ActionResult Webhook([FromBody] ApiNotification notification)
        {
            HomeController.notification = notification;
            return new OkObjectResult("");
        }

        [HttpPost]
        public async Task<ActionResult> RetrieveUpdates()
        {
            if (notification != null)
            {
                try
                {
                    MerchantOrderStatusResponse response = null;
                    do
                    {
                        response = await omniKassa.RetrieveAnnouncement(notification);
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
        public async Task<ActionResult> RetrievePaymentBrands()
        {
            try
            {
                PaymentBrandsResponse response = await omniKassa.RetrievePaymentBrands();
            }
            catch (RabobankSdkException)
            {

            }
            return View("Index");
        }

        private Dictionary<String, String> GetDictionary(NameValueCollection collection)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            for (int i = 0; i < collection.Count; i++)
            {
                string key = collection.GetKey(i);
                string value = collection[key];
                dictionary.Add(key, value);
            }
            return dictionary;
        }

        private Dictionary<String, String> GetDictionary(Dictionary<String, StringValues> collection)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            foreach(KeyValuePair<String, StringValues> pair in collection)
            {
                dictionary.Add(pair.Key, pair.Value);
            }
            return dictionary;
        }

        private NameValueCollection GetCollection(IFormCollection formCollection)
        {
            NameValueCollection collection = new NameValueCollection();
            for (int i = 0; i < formCollection.Count; i++)
            {
                string key = collection.GetKey(i);
                string value = collection[key];
                collection.Add(key, value);
            }
            return collection;
        }

        public MerchantOrder GetOrder()
        {
            Money itemAmount = Money.FromDecimal(Currency.EUR, 99.99m);
            Money itemTax = Money.FromDecimal(Currency.EUR, 4.99m);

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
