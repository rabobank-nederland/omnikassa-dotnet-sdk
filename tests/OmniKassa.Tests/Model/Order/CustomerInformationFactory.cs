using System;
using OmniKassa.Model.Enums;
using OmniKassa.Model.Order;

namespace OmniKassa.Tests.Model.Order
{
    public class CustomerInformationFactory
    {
        public static CustomerInformation CustomerInformationFull()
        {
            return CustomerInformation()
                    .WithDateOfBirth("01-01-1999")
                    .WithGender(Gender.M)
                    .WithEmailAddress("developer@example.com")
                    .WithInitials("d.")
                    .WithTelephoneNumber("0031204111111")
                    .WithFullName("Jan de Ruiter")
                    .Build();
        }

        public static CustomerInformation.Builder CustomerInformation()
        {
            return DefaultBuilder();
        }

        private static CustomerInformation.Builder DefaultBuilder()
        {
            return new CustomerInformation.Builder();
        }
    }
}
