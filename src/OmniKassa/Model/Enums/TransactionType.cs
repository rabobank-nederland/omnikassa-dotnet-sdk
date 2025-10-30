using System;
using Newtonsoft.Json;
using OmniKassa.Model.Order;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// Type of transaction
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Transaction is a normal payment
        /// </summary>
        PAYMENT,

        /// <summary>
        /// Transaction is a refund
        /// </summary>
        REFUND,

        /// <summary>
        /// An authorization for a transaction
        /// </summary>
        AUTHORIZE,

        /// <summary>
        /// Capture of a transaction
        /// </summary>
        CAPTURE
    }
}
