using System;
namespace OmniKassa.Tests
{
    public abstract class TestConfig
    {
        public static string SIGNING_KEY = "J+seXdXvw9SMuZu2sphZjRX+3p1gJfmZMenIjeQy+m0=";
        public static byte[] SIGNING_KEY_BYTES = Convert.FromBase64String(SIGNING_KEY);

        public static string TOKEN = "eyJraWQiOiIvKzdpVE5PL0FmSEhKN05kYmFWVGcyZTR6eXFjN3dYV3pFT08wcktoU0NJPSIsImFsZyI6IlJTMjU2In0.eyJta2lkIjoyNDI2NSwiZW52IjoiUyIsImV4cCI6NzI1ODAyODQwMH0.KnwaMEQSsAmF6Z1K7O6D8JMAPr6PgI16KYGUMv6jTO7BseX35N8C79jb8TjxVz27pX4d4jba1DpCPRE27zY70Y2OjDZOsiW9LZiTABe69ST8CchfrVXBQFtyiGLJis3L-fC21efntUiEfkM1m83UyDew5TIMADqMJOIV1G_lQ_rrzje5J35aVtLgIBUHi80cGqAyNoqBaZMspUhLK27pdybbIrFAPuS0rd_1K1tUhEB5MVnkF_N6KIlNotAdQdypT3WeUhfJtfEhn4SI9eFZ5dcz1ag6sdh_vZz174qkLOiLDvOOIuSXOXuLiM_dAS1pZepOJiREFqizNDkedpH48Q";
        public static string ROK_SERVER_URL = EnvironmentHelper.GetUrl(Environment.SANDBOX);

        public static string REDIRECT_URL_START = "https://betalen.rabobank.nl/omnikassa-sandbox/nl/payment-brand?token=";
		public static string OMNIKASSA_ORDER_ID = "25da863a-60a5-475d-ae47-c0e4bd1bec31";

        public static string RESOURCE_PREFIX = "omnikassa_dotnet_test.Resources.";
    }
}
