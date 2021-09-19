using OmniKassa.Model.Response;
using OmniKassa.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace omnikassa_dotnet_test.Model.Response
{
    public class IdealIssuersResponseTest
    {
        [Fact]
        public void asJson_Should_ReturnCorrectJsonObjectEmpty()
        {
            IdealIssuersResponse expected = TestHelper.GetObjectFromJsonFile<IdealIssuersResponse>("issuers_response_single.json");

            IdealIssuersResponse actual = CreateIdealIssuersResponse();

            Assert.Equal(expected, actual);
            Assert.Equal(expected.GetHashCode(), actual.GetHashCode());
        }

        private static IdealIssuersResponse CreateIdealIssuersResponse()
        {
            return new IdealIssuersResponse(
                new List<IdealIssuer>()
                {
                    new IdealIssuer
                    (
                        "ASNBNL21", 
                        "ASN Bank", 
                        new List<IdealIssuerLogo>()
                        {
                            new IdealIssuerLogo
                            (
                                "http://rabobank.nl/static/issuers/ASNBNL21.png",
                                "image/png"
                            )
                        },
                        "Nederland"
                    )
                }
            );
        }
    }
}
