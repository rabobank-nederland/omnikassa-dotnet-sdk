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
        SUCCESS,
        CANCELLED,
        EXPIRED,
        FAILURE,
        OPEN,
        NEW,
        ACCEPTED
    }
}
