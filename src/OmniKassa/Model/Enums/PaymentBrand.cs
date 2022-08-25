using OmniKassa.Model.Order;

namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// This enum houses the different types of payment brands that can be included in the <see cref="MerchantOrder"/> to restrict 
    /// the payment brands that the consumer can choose from.
    /// </summary>
    public enum PaymentBrand
    {
        /// <summary>
        /// iDEAL
        /// </summary>
        IDEAL,

        /// <summary>
        /// PayPal
        /// </summary>
        PAYPAL,

        /// <summary>
        /// AfterPay
        /// </summary>
        AFTERPAY,

        /// <summary>
        /// Mastercard
        /// </summary>
        MASTERCARD,

        /// <summary>
        /// Visa
        /// </summary>
        VISA,

        /// <summary>
        /// Bancontact
        /// </summary>
        BANCONTACT,

        /// <summary>
        /// Maestro
        /// </summary>
        MAESTRO,

        /// <summary>
        /// V Pay
        /// </summary>
        V_PAY,

        /// <summary>
        /// SOFORT
        /// </summary>
        SOFORT,

        /// <summary>
        /// Comprises MASTERCARD, VISA, BANCONTACT, MAESTRO and V_PAY.
        /// </summary>
        CARDS
    }
}
