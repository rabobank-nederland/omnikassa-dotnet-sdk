using System;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using Xunit;

namespace OmniKassa.Tests.Model.Order
{
    public class AddressTest
    {
        private Address address = AddressFactory.DefaultAddress();
        private Address addressFull = AddressFactory.AddressFull();

        [Fact]
        public void TestFields()
        {
            Assert.Equal("Developer", address.FirstName);
            Assert.Null(address.MiddleName);
            Assert.Equal("Doe", address.LastName);
            Assert.Equal("DeveloperStreet 1a", address.Street);
            Assert.Null(address.HouseNumber);
            Assert.Null(address.HouseNumberAddition);
            Assert.Equal("1234AB", address.PostalCode);
            Assert.Equal("Utrecht", address.City);
            Assert.Equal(CountryCode.NL, address.CountryCode);
        }

        [Fact]
        public void TestFieldsFull()
        {
            Assert.Equal("Developer", addressFull.FirstName);
            Assert.Equal("van", addressFull.MiddleName);
            Assert.Equal("Doe", addressFull.LastName);
            Assert.Equal("DeveloperStreet", addressFull.Street);
            Assert.Equal("1", addressFull.HouseNumber);
            Assert.Equal("a", addressFull.HouseNumberAddition);
            Assert.Equal("1234AB", addressFull.PostalCode);
            Assert.Equal("Utrecht", addressFull.City);
            Assert.Equal(CountryCode.NL, addressFull.CountryCode);
        }

        [Fact]
        public void TestEquals()
        {
            Address address1 = AddressFactory.DefaultAddress();
            Address address2 = AddressFactory.DefaultAddress();

            Assert.True(address1.Equals(address2));
            Assert.True(address1.GetHashCode() == address2.GetHashCode());

            Address address3 = AddressFactory.AddressFull();
            Address address4 = AddressFactory.AddressFull();

            Assert.True(address3.Equals(address4));
            Assert.True(address3.GetHashCode() == address4.GetHashCode());
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject()
        {
            Address expected = TestHelper.GetObjectFromJsonFile<Address>("address.json");
            Assert.Equal(expected, address);
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject_full()
        {
            Address expected = TestHelper.GetObjectFromJsonFile<Address>("address_full.json");
            Assert.Equal(expected, addressFull);
        }
    }
}
