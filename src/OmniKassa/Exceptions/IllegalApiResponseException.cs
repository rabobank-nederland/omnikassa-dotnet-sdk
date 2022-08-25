using System;
using Newtonsoft.Json;

namespace OmniKassa.Exceptions
{
    /// <summary>
    /// The response returned when an error occurred while executing an OmniKassa API request
    /// </summary>
    [Serializable]
    public class IllegalApiResponseException : RabobankSdkException
    {
        /// <summary>
        /// Error code of the exception
        /// </summary>
        public int ErrorCode { get; private set; }

        /// <summary>
        /// Error message of the exception
        /// </summary>
        public String ErrorMessage { get; private set; }

        /// <summary>
        /// Initializes an IllegalApiResponseException
        /// </summary>
        /// <param name="errorCode">Error code</param>
        /// <param name="errorMessage">Error message</param>
        public IllegalApiResponseException(int errorCode, String errorMessage) :
            base("The OmniKassa API returned message: " + errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes an IllegalApiResponseException
        /// </summary>
        /// <param name="errorCode">Error code</param>
        public IllegalApiResponseException(int errorCode) :
            base("The OmniKassa API returned errorCode: #" + errorCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = null;
        }

        /// <summary>
        /// Helper function to initialize an IllegalApiResponseException by parsing the JSON
        /// </summary>
        /// <param name="json">JSON containing the API error</param>
        /// <returns>New instance of IllegalApiResponseException</returns>
        public static IllegalApiResponseException Of(String json)
        {
            OmniKassaErrorResponse response = JsonConvert.DeserializeObject<OmniKassaErrorResponse>(json);

            if (response.ErrorCode == InvalidAccessTokenException.INVALID_AUTHORIZATION_ERROR_CODE)
            {
                if(response.ConsumerMessage != null &&
                   response.ConsumerMessage.ToLower().Equals("invalid or missing signature"))
                {
                    return new IllegalSignatureException(response.ErrorCode, response.ConsumerMessage);
                }
                else
                {
                    return GetInvalidAccessTokenException(response);
                }
            }

            if (response.ConsumerMessage != null)
            {
                return new IllegalApiResponseException(response.ErrorCode, response.ConsumerMessage);
            }
            return new IllegalApiResponseException(response.ErrorCode);
        }

        private static IllegalApiResponseException GetInvalidAccessTokenException(OmniKassaErrorResponse response)
        {
            if (response.ErrorMessage != null)
            {
                return new InvalidAccessTokenException(response.ErrorMessage);
            }
            return new InvalidAccessTokenException();
        }
    }
}
