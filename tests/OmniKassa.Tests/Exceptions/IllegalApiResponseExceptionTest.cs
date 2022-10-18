using OmniKassa.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace omnikassa_dotnet_test.Exceptions
{
    public class IllegalApiResponseExceptionTest
    {
        [Fact]
        public void InvalidAccessTokenExceptionWithCustomErrorMessage()
        {
            string json = "{ '" + OmniKassaErrorResponse.ERROR_CODE_FIELD_NAME + "': '" + InvalidAccessTokenException.INVALID_AUTHORIZATION_ERROR_CODE + "', 'errorMessage': 'my custom error message' }";
            IllegalApiResponseException actual = IllegalApiResponseException.Of(json);

            Assert.True(actual is InvalidAccessTokenException);
            Assert.Equal(InvalidAccessTokenException.INVALID_AUTHORIZATION_ERROR_CODE, actual.ErrorCode);
            Assert.Equal("my custom error message", actual.ErrorMessage);
        }
    }
}
