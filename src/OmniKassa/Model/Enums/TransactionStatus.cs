using System;
using Newtonsoft.Json;
using OmniKassa.Model.Order;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// Current status of transaction
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// The transaction was successful
        /// </summary>
        SUCCESS,

        /// <summary>
        /// The transaction was canceled
        /// </summary>
        CANCELLED,

        /// <summary>
        /// The transaction was expired
        /// </summary>
        EXPIRED,

        /// <summary>
        /// A failure occured
        /// </summary>
        FAILURE,

        /// <summary>
        /// The transaction is still open
        /// </summary>
        OPEN,

        /// <summary>
        /// The transaction is new
        /// </summary>
        NEW,

        /// <summary>
        /// The transaction was accepted
        /// </summary>
        ACCEPTED
    }
}
