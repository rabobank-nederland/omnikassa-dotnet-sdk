using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.RaboOmniKassa
{
    /// <summary>
    /// Represents settings of the Rabo OmniKassa payment plugin.
    /// </summary>
    public class RaboOmniKassaPaymentSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use sandbox (testing environment).
        /// </summary>
        public bool UseSandbox { get; set; }

        /// <summary>
        /// Gets or sets the key used to sign the message (can be copied from the Rabo OmniKassa dashboard).
        /// </summary>
        public string SigningKey { get; set; }

        /// <summary>
        /// Gets or sets the refresh token (can be copied from the Rabo OmniKassa dashboard).
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets an additional fee.
        /// </summary>
        public decimal AdditionalFee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }
    }
}