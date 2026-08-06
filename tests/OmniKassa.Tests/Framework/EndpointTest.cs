using System;
using Xunit;
using OmniKassa.Model;
using OmniKassa.Model.Request;
using OmniKassa.Model.Response;
using OmniKassa.Model.Enums;
using omnikassa_dotnet_test.Mocks;

namespace omnikassa_dotnet_test.Framework
{
    public class EndpointTest
    {
        private readonly TestableEndpoint endpoint;
        private readonly MockHttpClient mockHttpClient;
        private readonly MockTokenProvider mockTokenProvider;

        public EndpointTest()
        {
            mockHttpClient = new MockHttpClient();
            mockTokenProvider = new MockTokenProvider();
            endpoint = new TestableEndpoint(mockHttpClient, mockTokenProvider);
        }

        [Fact]
        public void InitiateRefundTransaction_WithValidToken_ReturnsRefundDetails()
        {
            var transactionId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var accessToken = "valid-access-token";
            var expected = new RefundDetailsResponse();

            mockTokenProvider.HasValidToken = true;
            mockTokenProvider.AccessToken = accessToken;
            mockHttpClient.PostRefundRequestResponse = expected;

            var refundRequest = new InitiateRefundRequest(Money.FromDecimal(Currency.EUR, 1.00m), "refund", null);
            var result = endpoint.InitiateRefundTransaction(refundRequest, transactionId, requestId);

            Assert.Equal(expected, result);
            Assert.Equal(accessToken, mockHttpClient.LastAccessTokenForPost);
            Assert.True(mockTokenProvider.HasNoValidAccessTokenCalled);
        }

        [Fact]
        public void InitiateRefundTransaction_WithInvalidToken_RetrievesNewTokenAndRetries()
        {
            var transactionId = Guid.NewGuid();
            var requestId = Guid.NewGuid();
            var oldToken = "old-token";
            var newTokenValue = "new-token";
            var newToken = new AccessToken("new-access-token", DateTime.UtcNow.AddHours(1), 3600);
            var expected = new RefundDetailsResponse();

            mockTokenProvider.HasValidToken = true;
            mockTokenProvider.AccessToken = oldToken;
            mockHttpClient.PostRefundRequestResponse = expected;
            mockHttpClient.ShouldThrowInvalidTokenOnFirstCallForPost = true;
            mockHttpClient.RetrieveNewTokenResponse = newToken;

            mockTokenProvider.SetupTokenRefresh(newTokenValue);

            var refundRequest = new InitiateRefundRequest(Money.FromDecimal(Currency.EUR, 1.00m), "refund", null);
            var result = endpoint.InitiateRefundTransaction(refundRequest, transactionId, requestId);

            Assert.Equal(expected, result);
            Assert.Equal(2, mockHttpClient.PostRefundRequestCallCount);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
        }

        [Fact]
        public void FetchRefundTransaction_WithInvalidToken_RetriesAfterRefresh()
        {
            var transactionId = Guid.NewGuid();
            var refundId = Guid.NewGuid();
            var oldToken = "old-token";
            var newToken = new AccessToken("refreshed", DateTime.UtcNow.AddHours(1), 3600);
            var expected = new RefundDetailsResponse();

            mockTokenProvider.HasValidToken = true;
            mockTokenProvider.AccessToken = oldToken;
            mockHttpClient.ShouldThrowInvalidTokenOnFirstCallForGet = true;
            mockHttpClient.GetRefundRequestResponse = expected;
            mockHttpClient.RetrieveNewTokenResponse = newToken;
            mockTokenProvider.SetupTokenRefresh("new-access-token");

            var result = endpoint.FetchRefundTransaction(transactionId, refundId);

            Assert.Equal(expected, result);
            Assert.Equal(2, mockHttpClient.GetRefundRequestCallCount);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
        }

        [Fact]
        public void FetchRefundableTransactionDetails_WithInvalidToken_RetriesAfterRefresh()
        {
            var transactionId = Guid.NewGuid();
            var oldToken = "old-token";
            var newToken = new AccessToken("refreshed", DateTime.UtcNow.AddHours(1), 3600);
            var expected = new TransactionRefundableDetailsResponse();

            mockTokenProvider.HasValidToken = true;
            mockTokenProvider.AccessToken = oldToken;
            mockHttpClient.ShouldThrowInvalidTokenOnFirstCallForGetRefundable = true;
            mockHttpClient.GetRefundableDetailsResponse = expected;
            mockHttpClient.RetrieveNewTokenResponse = newToken;
            mockTokenProvider.SetupTokenRefresh("new-access-token");

            var result = endpoint.FetchRefundableTransactionDetails(transactionId);

            Assert.Equal(expected, result);
            Assert.Equal(2, mockHttpClient.GetRefundableDetailsCallCount);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
        }
    }
}