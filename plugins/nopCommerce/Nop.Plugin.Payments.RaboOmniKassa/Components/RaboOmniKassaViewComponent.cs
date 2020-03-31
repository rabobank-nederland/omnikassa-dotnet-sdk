using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.RaboOmniKassa.Components
{
    [ViewComponent(Name = "PaymentRaboOmniKassa")]
    public class RaboOmniKassaViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Payments.RaboOmniKassa/Views/PaymentInfo.cshtml");
        }
    }
}
