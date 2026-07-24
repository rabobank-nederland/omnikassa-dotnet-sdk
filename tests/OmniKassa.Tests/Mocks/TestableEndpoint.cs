using System;
using System.Threading.Tasks;
using OmniKassa.Model;
using OmniKassa.Model.Response;
using OmniKassa.Exceptions;
using OmniKassa.Model.Request;

namespace omnikassa_dotnet_test.Mocks
{
    /// <summary>
    /// Testable endpoint that exposes private methods for testing
    /// </summary>
    public class TestableEndpoint
    {
        private readonly MockHttpClient httpClient;
        private readonly MockTokenProvider tokenProvider;

        public TestableEndpoint(MockHttpClient httpClient, MockTokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            this.tokenProvider = tokenProvider;
        }

        public async Task<OrderStatusResponse> RetrieveOrder(string orderId)
        {
            await ValidateAccessToken();

            try
            {
                return await httpClient.GetOrderById(orderId, tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                await RetrieveNewToken();
                return await httpClient.GetOrderById(orderId, tokenProvider.GetAccessToken());
            }
        }

        public async Task RetrieveNewToken()
        {
            AccessToken retrievedToken = await httpClient.RetrieveNewToken(tokenProvider.GetRefreshToken());
            tokenProvider.SetAccessToken(retrievedToken);
        }

        public async Task CallValidateAccessToken()
        {
            await ValidateAccessToken();
        }

        private async Task ValidateAccessToken()
        {
            if (tokenProvider.HasNoValidAccessToken())
            {
                await RetrieveNewToken();
            }
        }

        public RefundDetailsResponse InitiateRefundTransaction(
            InitiateRefundRequest refundRequest,
            Guid transactionId,
            Guid requestId)
        {
            ValidateAccessTokenSync();

            try
            {
                return httpClient.PostRefundRequest(
                    refundRequest,
                    transactionId,
                    requestId,
                    tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                RetrieveNewTokenSync();

                return httpClient.PostRefundRequest(
                    refundRequest,
                    transactionId,
                    requestId,
                    tokenProvider.GetAccessToken());
            }
        }

        public RefundDetailsResponse FetchRefundTransaction(
            Guid transactionId,
            Guid refundId)
        {
            ValidateAccessTokenSync();

            try
            {
                return httpClient.GetRefundRequest(
                    transactionId,
                    refundId,
                    tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                RetrieveNewTokenSync();

                return httpClient.GetRefundRequest(
                    transactionId,
                    refundId,
                    tokenProvider.GetAccessToken());
            }
        }

        public TransactionRefundableDetailsResponse FetchRefundableTransactionDetails(
            Guid transactionId)
        {
            ValidateAccessTokenSync();

            try
            {
                return httpClient.GetRefundableDetails(
                    transactionId,
                    tokenProvider.GetAccessToken());
            }
            catch (InvalidAccessTokenException)
            {
                RetrieveNewTokenSync();

                return httpClient.GetRefundableDetails(
                    transactionId,
                    tokenProvider.GetAccessToken());
            }
        }


        private void RetrieveNewTokenSync()
        {
            AccessToken retrievedToken =
                httpClient.RetrieveNewToken(tokenProvider.GetRefreshToken())
                          .GetAwaiter()
                          .GetResult();

            tokenProvider.SetAccessToken(retrievedToken);
        }


        private void ValidateAccessTokenSync()
        {
            if (tokenProvider.HasNoValidAccessToken())
            {
                RetrieveNewTokenSync();
            }
        }
    }
}
