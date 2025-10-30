using OmniKassa.Model;

namespace omnikassa_dotnet_test.Mocks
{
    /// <summary>
    /// Mock token provider for testing
    /// </summary>
    public class MockTokenProvider
    {
        public bool HasValidToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public AccessToken LastSetAccessToken { get; private set; }

        public bool HasNoValidAccessTokenCalled { get; private set; }
        public bool GetRefreshTokenCalled { get; private set; }
        public bool SetAccessTokenCalled { get; private set; }

        public bool HasNoValidAccessToken()
        {
            HasNoValidAccessTokenCalled = true;
            return !HasValidToken;
        }

        public string GetAccessToken()
        {
            return AccessToken;
        }

        public string GetRefreshToken()
        {
            GetRefreshTokenCalled = true;
            return RefreshToken;
        }

        public void SetAccessToken(AccessToken token)
        {
            SetAccessTokenCalled = true;
            LastSetAccessToken = token;
        }

        public void SetupTokenRefresh(string newAccessToken)
        {
            AccessToken = newAccessToken;
        }
    }
}
