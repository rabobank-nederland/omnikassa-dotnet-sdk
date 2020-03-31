using System;
namespace OmniKassa.Model.Enums
{
    /// <summary>
    /// Contains the different states a payment may have
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment was a success
        /// </summary>
        COMPLETED,

        /// <summary>
        /// Payment was canceled
        /// </summary>
        CANCELLED,

        /// <summary>
        /// Payment is in progress
        /// </summary>
        IN_PROGRESS,

        /// <summary>
        /// Payment session is expired
        /// </summary>
        EXPIRED
    }

    /// <summary>
    /// Helper class to convert a string representation to a <see cref="PaymentStatus"/>
    /// </summary>
    public static class PaymentStatusParser {

        /// <summary>
        /// Gets the PaymentStatus from a string representation
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>PaymentStatus</returns>
        public static PaymentStatus? GetStatus(String value) {
            switch(value) {
                case "COMPLETED":   return PaymentStatus.COMPLETED;
                case "CANCELLED":   return PaymentStatus.CANCELLED;
                case "IN_PROGRESS": return PaymentStatus.IN_PROGRESS;
                case "EXPIRED":     return PaymentStatus.EXPIRED;
            }
            return null;
        }
    }
}
