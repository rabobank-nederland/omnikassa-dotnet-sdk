#if NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0|| NETCOREAPP3_0
#define NETCORE
#endif

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OmniKassa;
using OmniKassa.Http;
using OmniKassa.Exceptions;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Response;
using OmniKassa.Utils;
using OmniKassa.Tests.Model.Order;
using OmniKassa.Tests.Model.Response;
using Xunit;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response.Notification;

namespace OmniKassa.Tests
{
    public class OmniKassaHttpClientTest
    {
        private static readonly string SIGNING_KEY_INVALID = "invalidinvalidin";

#if NETCORE
        [Fact]
        public async void DeprecatedAnnounceMerchantOrder_HappyFlow()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder order = MerchantOrderFactory.Any();
            MerchantOrderResponse response = await omniKassa.AnnounceMerchantOrder(order);
            Assert.StartsWith("https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=", response.RedirectUrl);
        }

        [Fact]
        public async void AnnounceMerchantOrder_HappyFlow()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder order = MerchantOrderFactory.Any();
            MerchantOrderResponse response = await omniKassa.Announce(order);
            Assert.StartsWith("https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=", response.RedirectUrl);
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out Guid guidOutput));
        }

        [Fact]
        public async void AnnounceMerchantOrder_HappyFlow_WithNegative()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder order = MerchantOrderFactory.WithNegativeItem();
            MerchantOrderResponse response = await omniKassa.Announce(order);
            Assert.StartsWith("https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=", response.RedirectUrl);
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out Guid guidOutput));
        }

        [Fact]
        public void AnnounceMerchantOrder_ApiReturnsError()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder.Builder builder = MerchantOrderFactory.DefaultBuilder();
            MerchantOrder order = builder.WithMerchantOrderId("!@#$%^").Build();
            Task<IllegalApiResponseException> ex = Assert.ThrowsAsync<IllegalApiResponseException>(
                async () => await omniKassa.Announce(order)
            );

            Assert.Equal(5017, ex.Result.ErrorCode);
            Assert.Equal("merchantOrderId should only contain alphanumeric characters", ex.Result.ErrorMessage);
        }

        [Fact]
        public async void GetPaymentBrands_HappyFlow()
        {
            Endpoint endpoint = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            PaymentBrandsResponse response = await endpoint.RetrievePaymentBrands();
            Assert.NotNull(response.PaymentBrands);
            Assert.True(response.PaymentBrands.Count > 0);

            foreach (PaymentBrandInfo brand in response.PaymentBrands)
            {
                Assert.NotEmpty(brand.Name);
                Assert.True(brand.IsActive);
            }
        }
#else
        [Fact]
        [Obsolete]
        public void DeprecatedAnnounceMerchantOrder_HappyFlow()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder order = MerchantOrderFactory.Any();
            String redirectUrl = omniKassa.AnnounceMerchantOrder(order);
            Assert.StartsWith("https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=", redirectUrl);
        }

        [Fact]
        public void AnnounceMerchantOrder_HappyFlow()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder order = MerchantOrderFactory.Any();
            MerchantOrderResponse response = omniKassa.Announce(order);
            Assert.StartsWith("https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=", response.RedirectUrl);

            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out Guid guidOutput));
        }

        [Fact]
        public void AnnounceMerchantOrder_HappyFlow_WithNegative()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder order = MerchantOrderFactory.WithNegativeItem();
            MerchantOrderResponse response = omniKassa.Announce(order);
            Assert.StartsWith("https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=", response.RedirectUrl);

            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out Guid guidOutput));
        }

        [Fact]
        public void AnnounceMerchantOrder_ApiReturnsError()
        {
            Endpoint endpoint = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrder.Builder builder = MerchantOrderFactory.DefaultBuilder();
            MerchantOrder order = builder.WithMerchantOrderId("!@#$%^").Build();
            IllegalApiResponseException ex = Assert.Throws<IllegalApiResponseException>(
                () => endpoint.Announce(order)
            );

            Assert.Equal(5017, ex.ErrorCode);
            Assert.Equal("merchantOrderId should only contain alphanumeric characters", ex.ErrorMessage);
        }

        [Fact]
        public void GetPaymentBrands_HappyFlow()
        {
            Endpoint endpoint = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            PaymentBrandsResponse response = endpoint.RetrievePaymentBrands();
            Assert.NotNull(response.PaymentBrands);
            Assert.True(response.PaymentBrands.Count > 0);

            foreach(PaymentBrandInfo brand in response.PaymentBrands)
            {
                Assert.NotEmpty(brand.Name);
                Assert.True(brand.IsActive);
            }
        }
#endif

        [Fact]
        public void GetAnnouncementData_HappyFlow()
        {
            Endpoint endpoint = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);

            MerchantOrderStatusResponseBuilder builder = new MerchantOrderStatusResponseBuilder();
            MerchantOrderStatusResponse response = builder.Build();

            Assert.False(response.MoreOrderResultsAvailable);
            Assert.Equal(2, response.OrderResults.Count);

            AssertFirstMerchantOrderResult(response.OrderResults[0]);
            AssertSecondMerchantOrderResult(response.OrderResults[1]);
        }

        private void AssertFirstMerchantOrderResult(MerchantOrderResult actual)
        {
            Assert.Equal(1, actual.PointOfInteractionId);
            Assert.Equal("MYSHOP0001", actual.MerchantOrderId);
            Assert.Equal("aec58605-edcf-4886-b12d-594a8a8eea60", actual.OmnikassaOrderId);
            Assert.Equal("", actual.ErrorCode);
            Assert.Equal(PaymentStatus.CANCELLED, actual.OrderStatus);
            Assert.Equal(DateTimeUtils.StringToDate("2016-07-28T12:51:15.574+02:00"), actual.OrderStatusDateTime);
            Assert.Equal(Currency.EUR, actual.PaidAmount.Currency);
            Assert.Equal(0L, actual.PaidAmount.GetAmountInCents());
            Assert.Equal(Currency.EUR, actual.TotalAmount.Currency);
            Assert.Equal(599L, actual.TotalAmount.GetAmountInCents());
        }

        private void AssertSecondMerchantOrderResult(MerchantOrderResult actual)
        {
            Assert.Equal(1, actual.PointOfInteractionId);
            Assert.Equal("MYSHOP0002", actual.MerchantOrderId);
            Assert.Equal("e516e630-9713-4cfa-ae88-c5fbc4b06744", actual.OmnikassaOrderId);
            Assert.Equal("", actual.ErrorCode);
            Assert.Equal(PaymentStatus.COMPLETED, actual.OrderStatus);
            Assert.Equal(DateTimeUtils.StringToDate("2016-07-28T13:58:50.205+02:00"), actual.OrderStatusDateTime);
            Assert.Equal(Currency.EUR, actual.PaidAmount.Currency);
            Assert.Equal(599L, actual.PaidAmount.GetAmountInCents());
            Assert.Equal(Currency.EUR, actual.TotalAmount.Currency);
            Assert.Equal(599L, actual.TotalAmount.GetAmountInCents());
        }

        [Fact]
        public void GetAnnouncementData_InvalidSignature()
        {
            Endpoint endpoint = Endpoint.Create(Environment.SANDBOX, SIGNING_KEY_INVALID, TestConfig.TOKEN);

            MerchantOrderStatusResponseBuilder builder = new MerchantOrderStatusResponseBuilder();
            MerchantOrderStatusResponse response = builder.Build();

            Assert.Throws<IllegalSignatureException>(() => response.ValidateSignature(SIGNING_KEY_INVALID));
        }

        [Fact]
        public void GetAnnouncementData_ApiReturnsError()
        {
            OmniKassaErrorResponse response = JsonConvert.DeserializeObject<OmniKassaErrorResponse>("{ errorCode: '5002', consumerMessage: '' }");
            String expectedJson = JsonConvert.SerializeObject(response);

            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(TestConfig.ROK_SERVER_URL, TestConfig.SIGNING_KEY_BYTES);
            Assert.Throws<IllegalApiResponseException>(() => httpClient.ProcessResult<ApiNotification>(expectedJson));
        }

#if NETCORE
        [Fact]
        public void GetAnnouncementData_ApiReturnsAuthenticationError()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);
            String expiry = DateTimeUtils.DateToString(DateTime.Now);
            ApiNotification notification = new ApiNotification(1, TestConfig.TOKEN, expiry, "event", "");
            notification.CalculateSignature(TestConfig.SIGNING_KEY_BYTES);

            Assert.ThrowsAsync<InvalidAccessTokenException>(async () => await omniKassa.RetrieveAnnouncement(notification));
        }

        [Fact]
        public async void RetrieveNewToken_HappyFlow()
        {
            byte[] signingKeyBytes = Convert.FromBase64String(TestConfig.SIGNING_KEY);
            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(TestConfig.ROK_SERVER_URL, signingKeyBytes);

            AccessToken accessToken = await httpClient.RetrieveNewToken(TestConfig.TOKEN);

            Assert.NotEmpty(accessToken.Token);
            Assert.Equal(3600000, accessToken.DurationInMillis);
            Assert.NotNull(accessToken.ValidUntil);
            Assert.True(accessToken.ValidUntil > DateTime.Now);
        }

        [Fact]
        public void RetrieveNewToken_SdkException()
        {
            byte[] signingKeyBytes = Convert.FromBase64String(TestConfig.SIGNING_KEY);
            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(TestConfig.ROK_SERVER_URL, signingKeyBytes);

            Assert.ThrowsAsync<InvalidAccessTokenException>(async () => await httpClient.RetrieveNewToken("invalid"));
        }
#else
        [Fact]
        public void GetAnnouncementData_ApiReturnsAuthenticationError()
        {
            Endpoint omniKassa = Endpoint.Create(Environment.SANDBOX, TestConfig.SIGNING_KEY, TestConfig.TOKEN);
            String expiry = DateTimeUtils.DateToString(DateTime.Now);
            ApiNotification notification = new ApiNotification(1, TestConfig.TOKEN, expiry, "event", "");
            notification.CalculateSignature(TestConfig.SIGNING_KEY_BYTES);

            Assert.Throws<InvalidAccessTokenException>(() => omniKassa.RetrieveAnnouncement(notification));
        }

        [Fact]
        public void RetrieveNewToken_HappyFlow()
        {
            byte[] signingKeyBytes = Convert.FromBase64String(TestConfig.SIGNING_KEY);
            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(TestConfig.ROK_SERVER_URL, signingKeyBytes);

            AccessToken accessToken = httpClient.RetrieveNewToken(TestConfig.TOKEN);

            Assert.NotEmpty(accessToken.Token);
            Assert.Equal(3600000, accessToken.DurationInMillis);
            Assert.NotNull(accessToken.ValidUntil);
            Assert.True(accessToken.ValidUntil > DateTime.Now);
        }

        [Fact]
        public void RetrieveNewToken_SdkException()
        {
            byte[] signingKeyBytes = Convert.FromBase64String(TestConfig.SIGNING_KEY);
            OmniKassaHttpClient httpClient = new OmniKassaHttpClient(TestConfig.ROK_SERVER_URL, signingKeyBytes);

            Assert.Throws<InvalidAccessTokenException>(() => httpClient.RetrieveNewToken("invalid"));
        }
#endif
    }
}
