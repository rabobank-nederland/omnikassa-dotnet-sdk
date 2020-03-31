using System;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// This field is used in combination with <see cref="PaymentBrand"/>.
    /// When <see cref="PaymentBrandForce"/> is set to <see cref="FORCE_ONCE"/> then the supplied <see cref="PaymentBrand"/> is only enforced in the customer's first
    /// payment attempt. If the payment was not successful then the consumer is allowed to select an alternative
    /// payment brand in the Hosted Payment Pages.
    /// When <see cref="PaymentBrandForce"/> is set to <see cref="FORCE_ALWAYS"/> then the consumer is not allowed to select an alternative
    /// payment brand. The customer is restricted to use the provided <see cref="PaymentBrand"/> for all payment attempts.
    /// </summary>
    public enum PaymentBrandForce
    {
        /// <summary>
        /// The supplied <see cref="PaymentBrand"/> is only enforced once
        /// </summary>
        FORCE_ONCE,

        /// <summary>
        /// The customer is restricted to the supplied <see cref="PaymentBrand"/>
        /// </summary>
        FORCE_ALWAYS
    }
}
