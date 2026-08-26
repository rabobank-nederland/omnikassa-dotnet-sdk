using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using OmniKassa.Http;
using Xunit;

namespace omnikassa_dotnet_test
{
    public class HttpClientHandlerTests
    {
        [Fact]
        public void Constructor_Creates_HttpClient_With_Defaults()
        {
            var client = new OmniKassaHttpClient("https://example.com/", new byte[] {1,2,3}, "ua", "pr");
            // use reflection to get private field mClient
            var field = typeof(OmniKassaHttpClient).GetField("mClient", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            var httpClient = field.GetValue(client) as HttpClient;
            Assert.NotNull(httpClient);
            Assert.Equal("https://example.com/", httpClient.BaseAddress.ToString());
            Assert.Contains(httpClient.DefaultRequestHeaders.Accept, h => h.MediaType == "application/json");
        }

#if NETFRAMEWORK
        [Fact]
        public void Constructor_DoesNotChangeSecurityProtocol()
        {
            var protocolBefore = ServicePointManager.SecurityProtocol;

            var client = new OmniKassaHttpClient(
                "https://example.com/",
                new byte[] { 1, 2, 3 },
                "ua",
                "pr");

            Assert.Equal(protocolBefore, ServicePointManager.SecurityProtocol);
        }

        [Fact]
        public void Constructor_DoesNotModifyCertificateCallback()
        {
            var callbackBefore = ServicePointManager.ServerCertificateValidationCallback;

            var client = new OmniKassaHttpClient(
                "https://example.com/",
                new byte[] { 1, 2, 3 },
                "ua",
                "pr");

            Assert.Same(
    callbackBefore,
    ServicePointManager.ServerCertificateValidationCallback);
        }
#endif
    }
}
