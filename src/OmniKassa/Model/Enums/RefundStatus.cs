using System;
using Newtonsoft.Json;
using OmniKassa.Model.Order;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// Status of refund
    /// </summary>
    public enum RefundStatus
    {
        /// <summary>
        /// The refund is pending
        /// </summary>
        PENDING,

        /// <summary>
        /// The refund is succeeded
        /// </summary>
        SUCCEEDED,

        /// <summary>
        /// The refund is failed
        /// </summary>
        FAILED,

        /// <summary>
        /// The status of the refund is unknown
        /// </summary>
        UNKNOWN
    }
}
