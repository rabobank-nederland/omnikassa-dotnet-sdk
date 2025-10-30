using System;
using System.Threading.Tasks;
using OmniKassa.Model;
using OmniKassa.Model.Response;
using OmniKassa.Exceptions;

namespace omnikassa_dotnet_test.Mocks
{
    /// <summary>
    /// Mock HTTP client for testing
    /// </summary>
    public class MockHttpClient
    {
        public OrderStatusResponse OrderByIdResponse { get; set; }
        public AccessToken RetrieveNewTokenResponse { get; set; }
        public Exception RetrieveNewTokenException { get; set; }
        public bool ShouldThrowInvalidTokenOnFirstCall { get; set; }

        public string LastOrderId { get; private set; }
        public string LastAccessToken { get; private set; }
        public int GetOrderByIdCallCount { get; private set; }
        public bool RetrieveNewTokenCalled { get; private set; }

        public async Task<OrderStatusResponse> GetOrderById(string orderId, string accessToken)
        {
            LastOrderId = orderId;
            LastAccessToken = accessToken;
            GetOrderByIdCallCount++;

            if (ShouldThrowInvalidTokenOnFirstCall && GetOrderByIdCallCount == 1)
            {
                throw new InvalidAccessTokenException();
            }

            return await Task.FromResult(OrderByIdResponse);
        }

        public async Task<AccessToken> RetrieveNewToken(string refreshToken)
        {
            RetrieveNewTokenCalled = true;

            if (RetrieveNewTokenException != null)
            {
                throw RetrieveNewTokenException;
            }

            return await Task.FromResult(RetrieveNewTokenResponse);
        }
    }
}
