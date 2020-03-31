using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.Payments.RaboOmniKassa
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes.
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            // Webhook Route
            routeBuilder.MapRoute("Plugin.Payments.RaboOmniKassa.Webhook", "Plugins/PaymentRaboOmniKassa/Webhook/",
                new { controller = "PaymentRaboOmniKassa", action = "WebhookEventHandler" });

            // Return Url Route
            routeBuilder.MapRoute("Plugin.Payments.RaboOmniKassa.ReturnUrl", "Plugins/PaymentRaboOmniKassa/ReturnUrl/",
                new { controller = "PaymentRaboOmniKassa", action = "ReturnUrlEventHandler" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return -1; }
        }
    }
}
