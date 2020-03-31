using System;
namespace OmniKassa.Exceptions
{
    /// <summary>
    /// Exception thrown when the access token is invalid
    /// </summary>
    public class InvalidAccessTokenException : IllegalApiResponseException
    {
        /// <summary>
        /// Error code for an invalid (authorization) token
        /// </summary>
        public static int INVALID_AUTHORIZATION_ERROR_CODE = 5001;

        /// <summary>
        /// Initializes an InvalidAccessTokenException
        /// </summary>
        /// <param name="message">Error message</param>
        public InvalidAccessTokenException(String message) :
            base(INVALID_AUTHORIZATION_ERROR_CODE, message)
        {
            
        }

        /// <summary>
        /// Initializes an InvalidAccessTokenException
        /// </summary>
        public InvalidAccessTokenException() :
            base(INVALID_AUTHORIZATION_ERROR_CODE)
        {
            
        }
    }
}
