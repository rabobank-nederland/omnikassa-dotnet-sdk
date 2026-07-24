using OmniKassa.Exceptions;
using OmniKassa.Model;
using OmniKassa.Model.Request;
using OmniKassa.Model.Response;
using System;
using System.Threading.Tasks;

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

        //
        // Sync members added so NETFRAMEWORK tests can reuse this mock
        //
        public RefundDetailsResponse PostRefundRequestResponse { get; set; }
        public RefundDetailsResponse GetRefundRequestResponse { get; set; }
        public TransactionRefundableDetailsResponse GetRefundableDetailsResponse { get; set; }

        public bool ShouldThrowInvalidTokenOnFirstCallForPost { get; set; }
        public bool ShouldThrowInvalidTokenOnFirstCallForGet { get; set; }
        public bool ShouldThrowInvalidTokenOnFirstCallForGetRefundable { get; set; }

        public string LastAccessTokenForPost { get; private set; }
        public string LastAccessTokenForGet { get; private set; }
        public string LastAccessTokenForGetRefundable { get; private set; }

        public int PostRefundRequestCallCount { get; private set; }
        public int GetRefundRequestCallCount { get; private set; }
        public int GetRefundableDetailsCallCount { get; private set; }

        public RefundDetailsResponse PostRefundRequest(InitiateRefundRequest refundRequest, Guid transactionId, Guid requestId, string token)
        {
            LastAccessTokenForPost = token;
            PostRefundRequestCallCount++;

            if (ShouldThrowInvalidTokenOnFirstCallForPost && PostRefundRequestCallCount == 1)
            {
                throw new InvalidAccessTokenException();
            }

            return PostRefundRequestResponse;
        }

        public RefundDetailsResponse GetRefundRequest(Guid transactionId, Guid refundId, string token)
        {
            LastAccessTokenForGet = token;
            GetRefundRequestCallCount++;

            if (ShouldThrowInvalidTokenOnFirstCallForGet && GetRefundRequestCallCount == 1)
            {
                throw new InvalidAccessTokenException();
            }

            return GetRefundRequestResponse;
        }

        public TransactionRefundableDetailsResponse GetRefundableDetails(Guid transactionId, string token)
        {
            LastAccessTokenForGetRefundable = token;
            GetRefundableDetailsCallCount++;

            if (ShouldThrowInvalidTokenOnFirstCallForGetRefundable && GetRefundableDetailsCallCount == 1)
            {
                throw new InvalidAccessTokenException();
            }

            return GetRefundableDetailsResponse;
        }
    }
}
