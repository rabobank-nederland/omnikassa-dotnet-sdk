using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using OmniKassa.Samples.DotNet461.Models;

namespace OmniKassa.Samples.DotNet461.Helpers
{
    public static class OrderHelper
    {
        public static string ISSUER_ID = "issuerId";

        public static OrderItem CreateOrderItem(NameValueCollection collection, int orderItemId)
        {
            String quantity = collection.Get("quantity");
            String name = collection.Get("name");
            String price = collection.Get("price");
            ItemCategory itemCategory = (ItemCategory)Enum.Parse(typeof(ItemCategory), collection.Get("category"));
            VatCategory vatCategory = (VatCategory)Enum.Parse(typeof(VatCategory), collection.Get("vat"));

            decimal priceDecimal = Convert.ToDecimal(price) / 100m;
            decimal taxDecimal = 0.0m;

            switch (vatCategory)
            {
                case VatCategory.HIGH:
                    taxDecimal = priceDecimal * 0.21m;
                    break;
                case VatCategory.LOW:
                    taxDecimal = priceDecimal * 0.06m;
                    break;
                case VatCategory.ZERO:
                    taxDecimal = 0.0m;
                    break;
            }

            priceDecimal += (decimal)taxDecimal;

            Money amount = Money.FromDecimal(Currency.EUR, Decimal.Round(priceDecimal, 2));
            Money tax = Money.FromDecimal(Currency.EUR, Decimal.Round(taxDecimal, 2));

            return new OrderItem.Builder()
                    .WithId(Convert.ToString(orderItemId))
                    .WithQuantity(Convert.ToInt32(quantity))
                    .WithName(name)
                    .WithDescription(name)
                    .WithAmount(amount)
                    .WithTax(tax)
                    .WithItemCategory(itemCategory)
                    .WithVatCategory(vatCategory)
                    .Build();
        }

        public static MerchantOrder PrepareOrder(NameValueCollection collection, WebShopModel model)
        {
            Decimal totalPrice = model.GetTotalPrice();
            if (totalPrice > 0.0m)
            {
                Address shippingDetails = CreateShippingDetails(collection);
                Address billingDetails = CreateBillingDetails(collection);
                CustomerInformation customerInformation = CreateCustomerInformation(collection);
                PaymentBrand? paymentBrand = CreatePaymentBrand(collection);
                PaymentBrandForce? paymentBrandForce = CreatePaymentBrandForce(collection);
                Dictionary<string, string> paymentBrandMetaData = CreatePaymentBrandMetaData(collection);
                string initiatingParty = GetInitiatingParty(collection);
                bool skipHppResultPage = GetSkipHppResultPage(collection);

                return model.PrepareMerchantOrder(
                    totalPrice,
                    customerInformation,
                    shippingDetails,
                    billingDetails,
                    paymentBrand,
                    paymentBrandForce,
                    paymentBrandMetaData,
                    initiatingParty,
                    skipHppResultPage
                );
            }
            else
            {
                model.Error = "Total amount must be greater than zero.";
            }
            return null;
        }

        private static CustomerInformation CreateCustomerInformation(NameValueCollection collection)
        {
            return new CustomerInformation.Builder()
                        .WithTelephoneNumber(collection.Get("phoneNumber"))
                        .WithInitials(collection.Get("initials"))
                        .WithGender(GetEnum<Gender>(collection.Get("gender")))
                        .WithEmailAddress(collection.Get("email"))
                        .WithDateOfBirth(collection.Get("birthDate"))
                        .WithFullName(collection.Get("fullName"))
                        .Build();
        }

        private static Address CreateBillingDetails(NameValueCollection collection)
        {
            return CreateAddressDetails(collection, "billing");
        }

        private static Address CreateShippingDetails(NameValueCollection collection)
        {
            return CreateAddressDetails(collection, "shipping");
        }

        private static Address CreateAddressDetails(NameValueCollection collection, String addressType)
        {
            String countryCode = collection.Get(addressType + "CountryCode");
            return new Address.Builder()
                    .WithFirstName(collection.Get(addressType + "FirstName"))
                    .WithMiddleName(collection.Get(addressType + "MiddleName"))
                    .WithLastName(collection.Get(addressType + "LastName"))
                    .WithStreet(collection.Get(addressType + "Street"))
                    .WithHouseNumber(collection.Get(addressType + "HouseNumber"))
                    .WithHouseNumberAddition(collection.Get(addressType + "HouseNumberAddition"))
                    .WithPostalCode(collection.Get(addressType + "PostalCode"))
                    .WithCity(collection.Get(addressType + "City"))
                    .WithCountryCode(GetEnum<CountryCode>(countryCode))
                    .Build();
        }

        private static PaymentBrand? CreatePaymentBrand(NameValueCollection collection)
        {
            String paymentBrand = collection.Get("paymentBrand");
            if (String.Equals("any", paymentBrand, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            return GetEnum<PaymentBrand>(paymentBrand);
        }

        private static PaymentBrandForce? CreatePaymentBrandForce(NameValueCollection collection)
        {
            String paymentBrandForce = collection.Get("paymentBrandForce");
            if (String.IsNullOrEmpty(paymentBrandForce))
                return null;
            return GetEnum<PaymentBrandForce>(paymentBrandForce);
        }

        private static Dictionary<string, string> CreatePaymentBrandMetaData(NameValueCollection collection)
        {
            String idealIssuer = collection.Get("idealIssuer");
            if (String.IsNullOrEmpty(idealIssuer))
                return null;
            return new Dictionary<string, string>() {
                { ISSUER_ID, idealIssuer }
            };
        }

        private static string GetInitiatingParty(NameValueCollection collection)
        {
            return collection.Get("initiatingParty");
        }

        private static bool GetSkipHppResultPage(NameValueCollection collection)
        {
            return collection.Get("skipHppResultPage") == "on";
        }

        public static T GetEnum<T>(String value)
        {
            if (String.IsNullOrEmpty(value))
                return default(T);

            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
