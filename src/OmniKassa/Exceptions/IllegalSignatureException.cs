using System;
namespace OmniKassa.Exceptions
{
    /// <summary>
    /// Exception thrown when a signature could not be validated by the SDK or API
    /// </summary>
    public class IllegalSignatureException : IllegalApiResponseException
    {
        /// <summary>
        /// Initializes an IllegalSignatureException
        /// </summary>
        public IllegalSignatureException() :
            base(0, "The signature validation of the response failed. Please contact the Rabobank service team.")
        {

        }

        /// <summary>
        /// Initializes an IllegalSignatureException
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="errorMessage"></param>
        public IllegalSignatureException(int errorCode, String errorMessage) :
            base(errorCode, errorMessage)
        {
            
        }
    }
}
