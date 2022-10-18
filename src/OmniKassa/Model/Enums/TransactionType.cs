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
        PAYMENT,
        REFUND,
        AUTHORIZE,
        CAPTURE
    }
}
