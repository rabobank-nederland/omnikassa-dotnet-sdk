using System;
namespace OmniKassa.Exceptions
{
    /// <summary>
    /// Represents errors that occur during OmniKassa SDK execution.
    /// </summary>
    [Serializable]
    public class RabobankSdkException : Exception
    {
        /// <summary>
        /// Initializes an OmniKassa exception
        /// </summary>
        /// <param name="message">Exception message</param>
        public RabobankSdkException(String message) :
            base(message)
        {
            
        }

        /// <summary>
        /// Initializes an OmniKassa exception
        /// </summary>
        /// <param name="cause">Exception object</param>
        public RabobankSdkException(Exception cause) :
            base(cause.Message)
        {

        }
    }
}
