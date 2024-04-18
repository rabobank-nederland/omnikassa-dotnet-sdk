using example_dotnet60.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Model.Response;
using System.Collections.Generic;

namespace example_dotnet60.Controllers
{
    public class WebShopViewData
    {
        public static List<SelectListItem> GetGenderItems(MerchantOrder order)
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "", Value = "" }
            };
            foreach (Gender item in typeof(Gender).GetEnumValues())
            {
                items.Add(new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = item.ToString(),
                    Selected = (item == order.CustomerInformation.Gender)
                });
            }
            return items;
        }

        public static List<SelectListItem> GetPaymentBrandItems(MerchantOrder order)
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "ANY", Value = "ANY" }
            };
            foreach (PaymentBrand item in typeof(PaymentBrand).GetEnumValues())
            {
                items.Add(new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = item.ToString(),
                    Selected = (item == order.PaymentBrand)
                });
            }
            return items;
        }

        public static List<SelectListItem> GetPaymentBrandForceItems(MerchantOrder order)
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "", Value = "" }
            };
            foreach (PaymentBrandForce item in typeof(PaymentBrandForce).GetEnumValues())
            {
                items.Add(new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = item.ToString(),
                    Selected = (item == order.PaymentBrandForce)
                });
            }
            return items;
        }

        public static List<SelectListItem> GetIdealIssuerItems(WebShopModel model)
        {
            var items = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "", Value = "" }
            };
            foreach (IdealIssuer item in model.GetIdealIssuers())
            {
                items.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            }
            return items;
        }

        public static List<SelectListItem> GetShippingAddressCountryItems(MerchantOrder order)
        {
            return GetCountryItems(order.ShippingDetails.CountryCode);
        }

        public static List<SelectListItem> GetBillingAddressCountryItems(MerchantOrder order)
        {
            return GetCountryItems(order.BillingDetails.CountryCode);
        }

        public static List<SelectListItem> GetCountryItems(CountryCode selected)
        {
            var items = new List<SelectListItem>();
            foreach (CountryCode item in typeof(CountryCode).GetEnumValues())
            {
                items.Add(new SelectListItem()
                {
                    Value = item.ToString(),
                    Text = item.ToString(),
                    Selected = (item == selected)
                });
            }
            return items;
        }
    }
}
