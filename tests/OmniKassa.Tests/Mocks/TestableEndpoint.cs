using System;
using System.Threading.Tasks;
using OmniKassa.Model;
using OmniKassa.Model.Response;
using OmniKassa.Exceptions;

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
    }
}
