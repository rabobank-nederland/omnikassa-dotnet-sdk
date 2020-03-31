using System;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;
using Xunit;

namespace OmniKassa.Tests.Model.Order
{
    public class CustomerInformationTest
    {
        private CustomerInformation customerInformation = CustomerInformationFactory.CustomerInformation().Build();
        private CustomerInformation customerInformationFull = CustomerInformationFactory.CustomerInformationFull();

        [Fact]
        public void TestFields()
        {
            Assert.Null(customerInformation.DateOfBirth);
            Assert.Null(customerInformation.EmailAddress);
            Assert.Null(customerInformation.Gender);
            Assert.Null(customerInformation.Initials);
            Assert.Null(customerInformation.TelephoneNumber);
        }

        [Fact]
        public void TestFieldsFull()
        {
            Assert.Equal("01-01-1999", customerInformationFull.DateOfBirth);
            Assert.Equal("developer@example.com", customerInformationFull.EmailAddress);
            Assert.Equal(Gender.M, customerInformationFull.Gender);
            Assert.Equal("d.", customerInformationFull.Initials);
            Assert.Equal("0031204111111", customerInformationFull.TelephoneNumber);
        }

        [Fact]
        public void TestEquals()
        {
            CustomerInformation ci1 = CustomerInformationFactory.CustomerInformation().Build();
            CustomerInformation ci2 = CustomerInformationFactory.CustomerInformation().Build();

            Assert.True(ci1.Equals(ci2));
            Assert.True(ci1.GetHashCode() == ci2.GetHashCode());

            CustomerInformation ci3 = CustomerInformationFactory.CustomerInformationFull();
            CustomerInformation ci4 = CustomerInformationFactory.CustomerInformationFull();

            Assert.True(ci3.Equals(ci4));
            Assert.True(ci3.GetHashCode() == ci4.GetHashCode());
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject()
        {
            String actualJson = JsonConvert.SerializeObject(customerInformation);
            String expectedJson = "{\"emailAddress\":null,\"dateOfBirth\":null,\"initials\":null,\"telephoneNumber\":null,\"gender\":null}";

            Assert.Equal(expectedJson, actualJson, true);
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject_full()
        {
            CustomerInformation expected = TestHelper.GetObjectFromJsonFile<CustomerInformation>("customer_information_full.json");
            Assert.Equal(expected, customerInformationFull);
        }
    }
}
