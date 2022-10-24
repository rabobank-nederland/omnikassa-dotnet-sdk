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
        PENDING,
        SUCCEEDED,
        FAILED,
        UNKNOWN
    }
}
