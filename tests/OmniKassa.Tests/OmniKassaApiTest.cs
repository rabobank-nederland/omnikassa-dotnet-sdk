#if NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP2_0 || NETCOREAPP3_0
#define NETCORE
#endif

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OmniKassa;
using OmniKassa.Exceptions;
using OmniKassa.Http;
using OmniKassa.Model;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;
using OmniKassa.Model.Response.Notification;
using OmniKassa.Tests.Connector;
using OmniKassa.Tests.Model;
using OmniKassa.Tests.Model.Response;
using Xunit;

namespace OmniKassa.Tests
{
    public class OmniKassaApiTest
    {
        private static readonly String SIGNING_KEY_FAKE = "c2VjcmV0";
        private static readonly byte[] SIGNING_KEY_FAKE_BYTES = Convert.FromBase64String(SIGNING_KEY_FAKE);

        private OmniKassaHttpClient mHttpClient;

        private Endpoint mOmniKassa;
        private TokenProviderSpy mTokenProvider;

        public OmniKassaApiTest()
        {
            InitializeTokenProvider();
            mHttpClient = new OmniKassaHttpClient(TestConfig.ROK_SERVER_URL, TestConfig.SIGNING_KEY_BYTES);
            mOmniKassa = Endpoint.Create(mHttpClient, mTokenProvider);
        }

        private void InitializeTokenProvider()
        {
            Dictionary<TokenProvider.FieldName, String> map = new Dictionary<TokenProvider.FieldName, String>
            {
                { TokenProvider.FieldName.REFRESH_TOKEN, TestConfig.TOKEN }
            };
            mTokenProvider = new TokenProviderSpy(map);
        }

#if NETCORE
        [Fact]
        public async void DeprecatedAnnounceMerchantOrder_HappyFlow()
        {
            MerchantOrderResponse response = await mOmniKassa.AnnounceMerchantOrder(MerchantOrderFactory.Any());

            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);
        }

        [Fact]
        public async void AnnounceMerchantOrder_HappyFlow()
        {
            MerchantOrder order = MerchantOrderFactory.Any();
            MerchantOrderResponse response = await mOmniKassa.Announce(order);

            Assert.True(DateTime.TryParse(order.Timestamp, out DateTime timestamp));

            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);

            Guid guidOutput;
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out guidOutput));
        }

        [Fact]
        public async void AnnounceMerchantOrder_AccessTokenExpired()
        {
            await mOmniKassa.RetrieveNewToken();

            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-1);
            mTokenProvider.SetValidUntil(dateTime);

            MerchantOrderResponse response = await mOmniKassa.Announce(MerchantOrderFactory.Any());
            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);

            Guid guidOutput;
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out guidOutput));
        }

        [Fact]
        public async void AnnounceMerchantOrder_UnexpectedAccessTokenExpired()
        {
            await mOmniKassa.RetrieveNewToken();

            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-1);
            mTokenProvider.SetValidUntil(dateTime);

            MerchantOrderResponse response = await mOmniKassa.Announce(MerchantOrderFactory.Any());
            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);

            Guid guidOutput;
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out guidOutput));
        }

        private async Task<MerchantOrderResponse> DoAnnounceMerchantOrder(MerchantOrder merchantOrder)
        {
            return await mHttpClient.AnnounceMerchantOrder(merchantOrder, mTokenProvider.GetAccessToken());
        }

        [Fact]
        public async void AnnounceMerchantOrder_UnexpectedAccessTokenExpiredTwice()
        {
            MerchantOrder merchantOrder = MerchantOrderFactory.Any();

            try
            {
                await DoAnnounceMerchantOrder(merchantOrder);
            }
            catch (InvalidAccessTokenException)
            {
                //Usually we would retrieve a new token first, but we keep using the invalid token to test this scenario.
                await Assert.ThrowsAsync<InvalidAccessTokenException>(async () => await DoAnnounceMerchantOrder(merchantOrder));
            }
        }
#else
        [Fact]
        [Obsolete]
        public void DeprecatedAnnounceMerchantOrder_HappyFlow()
        {
            String redirectUrl = mOmniKassa.AnnounceMerchantOrder(MerchantOrderFactory.Any());

            Assert.StartsWith(TestConfig.REDIRECT_URL_START, redirectUrl);
        }

        [Fact]
        public void AnnounceMerchantOrder_HappyFlow()
        {
            MerchantOrder order = MerchantOrderFactory.Any();
            MerchantOrderResponse response = mOmniKassa.Announce(order);

            Assert.True(DateTime.TryParse(order.Timestamp, out DateTime timestamp));

            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);

            Guid guidOutput;
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out guidOutput));
        }

        [Fact]
        public void AnnounceMerchantOrder_AccessTokenExpired()
        {
            mOmniKassa.RetrieveNewToken();

            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-1);
            mTokenProvider.SetValidUntil(dateTime);

            MerchantOrderResponse response = mOmniKassa.Announce(MerchantOrderFactory.Any());
            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);

            Guid guidOutput;
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out guidOutput));
        }

        [Fact]
        public void AnnounceMerchantOrder_UnexpectedAccessTokenExpired()
        {
            mOmniKassa.RetrieveNewToken();

            DateTime dateTime = DateTime.Now;
            dateTime = dateTime.AddDays(-1);
            mTokenProvider.SetValidUntil(dateTime);

            MerchantOrderResponse response = mOmniKassa.Announce(MerchantOrderFactory.Any());
            Assert.StartsWith(TestConfig.REDIRECT_URL_START, response.RedirectUrl);

            Guid guidOutput;
            Assert.True(Guid.TryParse(response.OmnikassaOrderId, out guidOutput));
        }

        private String DoAnnounceMerchantOrder(MerchantOrder merchantOrder)
        {
            MerchantOrderResponse response = mHttpClient.AnnounceMerchantOrder(merchantOrder, mTokenProvider.GetAccessToken());
            return response.RedirectUrl;
        }

        [Fact]
        public void AnnounceMerchantOrder_UnexpectedAccessTokenExpiredTwice()
        {
            MerchantOrder merchantOrder = MerchantOrderFactory.Any();

            try
            {
                DoAnnounceMerchantOrder(merchantOrder);
            }
            catch (InvalidAccessTokenException)
            {
                //Usually we would retrieve a new token first, but we keep using the invalid token to test this scenario.
                Assert.Throws<InvalidAccessTokenException>(() => DoAnnounceMerchantOrder(merchantOrder));
            }
        }
#endif

        [Fact]
        public void RetrieveAnnouncement_HappyFlow() 
        {
            MerchantOrderStatusResponse expected = GetOrderStatusResponseWithValidSignature();

            OmniKassaHttpClient httpClient = new OmniKassaHttpClient("http://redirectUrl", SIGNING_KEY_FAKE_BYTES);

            String json = JsonConvert.SerializeObject(expected);
            MerchantOrderStatusResponse actual = httpClient.ProcessResult<MerchantOrderStatusResponse>(json);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void RetrieveAnnouncement_InValidSignature() 
        {
            MerchantOrderStatusResponse response = GetOrderStatusResponseWithInValidSignature();

            OmniKassaHttpClient httpClient = new OmniKassaHttpClient("http://redirectUrl", SIGNING_KEY_FAKE_BYTES);

            String json = JsonConvert.SerializeObject(response);
            Assert.Throws<IllegalSignatureException>(() => httpClient.ProcessResult<MerchantOrderStatusResponse>(json));
        }

        private MerchantOrderStatusResponse GetOrderStatusResponseWithValidSignature()
        {
            return new MerchantOrderStatusResponseBuilder().Build();
        }

        private MerchantOrderStatusResponse GetOrderStatusResponseWithInValidSignature()
        {
            return new MerchantOrderStatusResponseBuilder().WithSignature("a2c8078188d1ed28e9dc54ebfcad9f712558fab329d202f").Build();
        }

        private MerchantOrderResponse PrepareMerchantOrderResponseWithValidSignature()
        {
            return new AnnounceOrderResponseBuilder()
                    .WithRedirectUrl("http://redirectUrl")
                    .WithOmnikassaOrderId("25da863a-60a5-475d-ae47-c0e4bd1bec31")
                    .Build();
        }

        private ApiNotification PrepareOrderStatusNotification()
        {
            return new OrderStatusNotificationBuilder()
                    .WithSignature("b3aca76859ff5ede10543b5c116446ed79ae0d815b8d063ca40dd5dfed79f49ca57bc87fbaafb7bb2759512377ba9b177fe75c8f83614fe8b3cc46821177bdce")
                    .Build();
        }
    }
}
