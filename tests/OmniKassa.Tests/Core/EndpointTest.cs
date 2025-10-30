using System;
using System.Threading.Tasks;
using Xunit;
using OmniKassa.Model;
using OmniKassa.Model.Response;
using omnikassa_dotnet_test.Mocks;

namespace omnikassa_dotnet_test.Core
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

        #region RetrieveOrder Tests

        [Fact]
        public async Task RetrieveOrder_WithValidToken_ReturnsOrderStatusResponse()
        {
            // Arrange
            var orderId = "test-order-123";
            var accessToken = "valid-access-token";
            var expectedResponse = new OrderStatusResponse();

            mockTokenProvider.HasValidToken = true;
            mockTokenProvider.AccessToken = accessToken;
            mockHttpClient.OrderByIdResponse = expectedResponse;

            // Act
            var result = await endpoint.RetrieveOrder(orderId);

            // Assert
            Assert.Equal(expectedResponse, result);
            Assert.Equal(orderId, mockHttpClient.LastOrderId);
            Assert.Equal(accessToken, mockHttpClient.LastAccessToken);
            Assert.True(mockTokenProvider.HasNoValidAccessTokenCalled);
        }

        [Fact]
        public async Task RetrieveOrder_WithInvalidToken_RetrievesNewTokenAndRetries()
        {
            // Arrange
            var orderId = "test-order-123";
            var oldAccessToken = "invalid-access-token";
            var newAccessToken = "new-valid-access-token";
            var refreshToken = "refresh-token";
            var newToken = new AccessToken("new-token", DateTime.UtcNow.AddHours(1), 3600);
            var expectedResponse = new OrderStatusResponse();

            mockTokenProvider.HasValidToken = true;
            mockTokenProvider.AccessToken = oldAccessToken;
            mockTokenProvider.RefreshToken = refreshToken;
            mockHttpClient.ShouldThrowInvalidTokenOnFirstCall = true;
            mockHttpClient.OrderByIdResponse = expectedResponse;
            mockHttpClient.RetrieveNewTokenResponse = newToken;

            // Mock token provider to return new token after refresh
            mockTokenProvider.SetupTokenRefresh(newAccessToken);

            // Act
            var result = await endpoint.RetrieveOrder(orderId);

            // Assert
            Assert.Equal(expectedResponse, result);
            Assert.Equal(2, mockHttpClient.GetOrderByIdCallCount);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
        }

        [Fact]
        public async Task RetrieveOrder_WithExpiredToken_RetrievesNewTokenFirst()
        {
            // Arrange
            var orderId = "test-order-123";
            var newAccessToken = "new-valid-access-token";
            var refreshToken = "refresh-token";
            var newToken = new AccessToken("new-token", DateTime.UtcNow.AddHours(1), 3600);
            var expectedResponse = new OrderStatusResponse();

            mockTokenProvider.HasValidToken = false; // Token is expired
            mockTokenProvider.RefreshToken = refreshToken;
            mockHttpClient.OrderByIdResponse = expectedResponse;
            mockHttpClient.RetrieveNewTokenResponse = newToken;
            mockTokenProvider.SetupTokenRefresh(newAccessToken);

            // Act
            var result = await endpoint.RetrieveOrder(orderId);

            // Assert
            Assert.Equal(expectedResponse, result);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
            Assert.Equal(1, mockHttpClient.GetOrderByIdCallCount);
        }

        #endregion

        #region RetrieveNewToken Tests

        [Fact]
        public async Task RetrieveNewToken_RetrievesAndSetsNewToken()
        {
            // Arrange
            var refreshToken = "refresh-token";
            var newToken = new AccessToken("new-access-token", DateTime.UtcNow.AddHours(1), 3600);

            mockTokenProvider.RefreshToken = refreshToken;
            mockHttpClient.RetrieveNewTokenResponse = newToken;

            // Act
            await endpoint.RetrieveNewToken();

            // Assert
            Assert.True(mockTokenProvider.GetRefreshTokenCalled);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
            Assert.Equal(newToken, mockTokenProvider.LastSetAccessToken);
        }

        [Fact]
        public async Task RetrieveNewToken_WithHttpClientException_ThrowsException()
        {
            // Arrange
            var refreshToken = "refresh-token";
            var expectedException = new Exception("Network error");

            mockTokenProvider.RefreshToken = refreshToken;
            mockHttpClient.RetrieveNewTokenException = expectedException;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => endpoint.RetrieveNewToken());
            Assert.Equal("Network error", exception.Message);
            Assert.False(mockTokenProvider.SetAccessTokenCalled);
        }

        #endregion

        #region ValidateAccessToken Tests

        [Fact]
        public async Task ValidateAccessToken_WithValidToken_DoesNotRetrieveNewToken()
        {
            // Arrange
            mockTokenProvider.HasValidToken = true;

            // Act
            await endpoint.CallValidateAccessToken();

            // Assert
            Assert.True(mockTokenProvider.HasNoValidAccessTokenCalled);
            Assert.False(mockHttpClient.RetrieveNewTokenCalled);
        }

        [Fact]
        public async Task ValidateAccessToken_WithInvalidToken_RetrievesNewToken()
        {
            // Arrange
            var refreshToken = "refresh-token";
            var newToken = new AccessToken("new-access-token", DateTime.UtcNow.AddHours(1), 3600);

            mockTokenProvider.HasValidToken = false;
            mockTokenProvider.RefreshToken = refreshToken;
            mockHttpClient.RetrieveNewTokenResponse = newToken;

            // Act
            await endpoint.CallValidateAccessToken();

            // Assert
            Assert.True(mockTokenProvider.HasNoValidAccessTokenCalled);
            Assert.True(mockHttpClient.RetrieveNewTokenCalled);
            Assert.True(mockTokenProvider.SetAccessTokenCalled);
        }

        #endregion
    }
}