using System;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;

namespace OmniKassa.Tests.Model.Order
{
    public class AddressFactory
    {
        public static Address AddressFull()
        {
            return DefaultBuilder()
                    .WithMiddleName("van")
                    .WithStreet("DeveloperStreet")
                    .WithHouseNumber("1")
                    .WithHouseNumberAddition("a")
                    .Build();
        }

        public static Address DefaultAddress()
        {
            return DefaultBuilder().Build();
        }

        private static Address.Builder DefaultBuilder()
        {
            return new Address.Builder()
                    .WithFirstName("Developer")
                    .WithLastName("Doe")
                    .WithStreet("DeveloperStreet 1a")
                    .WithPostalCode("1234AB")
                    .WithCity("Utrecht")
                    .WithCountryCode(CountryCode.NL);
        }
    }
}
