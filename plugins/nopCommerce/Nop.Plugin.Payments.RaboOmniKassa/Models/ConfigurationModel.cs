using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Mvc.Models;

namespace Nop.Plugin.Payments.RaboOmniKassa.Models
{
    /// <summary>
    /// Represents the Rabo OmniKassa configuration model.
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RaboOmniKassa.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }
        public bool UseSandbox_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RaboOmniKassa.Fields.SigningKey")]
        public string SigningKey { get; set; }
        public bool SigningKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RaboOmniKassa.Fields.RefreshToken")]
        public string RefreshToken { get; set; }
        public bool RefreshToken_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.RaboOmniKassa.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }
    }
}