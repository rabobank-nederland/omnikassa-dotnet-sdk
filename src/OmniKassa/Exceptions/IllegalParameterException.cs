using System;
using System.Collections.Generic;
using System.Text;

namespace OmniKassa.Exceptions
{
    /// <summary>
    /// This exception is typically thrown when a parameter in a request contains an invalid value.
    /// </summary>
    public class IllegalParameterException : RabobankSdkException
    {
        /// <summary>
        /// Construct with message
        /// </summary>
        /// <param name="message">Message</param>
        public IllegalParameterException(String message) :
            base(message)
        {
            
        }

        /// <summary>
        /// Construct with Exception
        /// </summary>
        /// <param name="cause">Exception</param>
        public IllegalParameterException(Exception cause) :
            base(cause)
        {
            
        }
    }
}
