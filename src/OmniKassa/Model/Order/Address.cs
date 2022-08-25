using System;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Order
{
    /// <summary>
    /// Address
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Address
    {
        /// <summary>
        /// First name
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public String FirstName { get; private set; }

        /// <summary>
        /// Middle name
        /// </summary>
        [JsonProperty(PropertyName = "middleName")]
        public String MiddleName { get; private set; }

        /// <summary>
        /// Last name
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public String LastName { get; private set; }

        /// <summary>
        /// Street
        /// </summary>
        [JsonProperty(PropertyName = "street")]
        public String Street { get; private set; }

        /// <summary>
        /// House number
        /// </summary>
        [JsonProperty(PropertyName = "houseNumber")]
        public String HouseNumber { get; private set; }

        /// <summary>
        /// House number addition
        /// </summary>
        [JsonProperty(PropertyName = "houseNumberAddition")]
        public String HouseNumberAddition { get; private set; }

        /// <summary>
        /// Postal code
        /// </summary>
        [JsonProperty(PropertyName = "postalCode")]
        public String PostalCode { get; private set; }

        /// <summary>
        /// City
        /// </summary>
        [JsonProperty(PropertyName = "city")]
        public String City { get; private set; }

        /// <summary>
        /// Country code
        /// </summary>
        [JsonProperty(PropertyName = "countryCode")]
        [JsonConverter(typeof(EnumJsonConverter<CountryCode>))]
        public CountryCode CountryCode { get; private set; }

        /// <summary>
        /// Initializes an empty Address
        /// </summary>
        public Address()
        {
            
        }

        /// <summary>
        /// Initializes an Address using the Builder
        /// </summary>
        /// <param name="builder">Builder</param>
        public Address(Builder builder)
        {
            FirstName = builder.FirstName;
            MiddleName = builder.MiddleName;
            LastName = builder.LastName;
            Street = builder.Street;
            HouseNumber = builder.HouseNumber;
            HouseNumberAddition = builder.HouseNumberAddition;
            PostalCode = builder.PostalCode;
            City = builder.City;
            CountryCode = builder.CountryCode;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override Boolean Equals(object obj)
        {
            if (this == obj)
            {
                return true;
            }
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Address))
            {
                return false;
            }
            Address address = (Address)obj;
            return Equals(FirstName, address.FirstName) &&
                   Equals(MiddleName, address.MiddleName) &&
                   Equals(LastName, address.LastName) &&
                   Equals(Street, address.Street) &&
                   Equals(HouseNumber, address.HouseNumber) &&
                   Equals(HouseNumberAddition, address.HouseNumberAddition) &&
                   Equals(PostalCode, address.PostalCode) &&
                   Equals(City, address.City) &&
                   CountryCode == address.CountryCode;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 0x51ed270b;
                hash = (hash * -1521134295) + (FirstName == null ? 0 : FirstName.GetHashCode());
                hash = (hash * -1521134295) + (MiddleName == null ? 0 : MiddleName.GetHashCode());
                hash = (hash * -1521134295) + (LastName == null ? 0 : LastName.GetHashCode());
                hash = (hash * -1521134295) + (Street == null ? 0 : Street.GetHashCode());
                hash = (hash * -1521134295) + (HouseNumber == null ? 0 : HouseNumber.GetHashCode());
                hash = (hash * -1521134295) + (HouseNumberAddition == null ? 0 : HouseNumberAddition.GetHashCode());
                hash = (hash * -1521134295) + (PostalCode == null ? 0 : PostalCode.GetHashCode());
                hash = (hash * -1521134295) + (City == null ? 0 : City.GetHashCode());
                hash = (hash * -1521134295) + CountryCode.GetHashCode();
                return hash;
            }
        }

        /// <summary>
        /// Address builder
        /// </summary>
        public class Builder
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public String FirstName { get; private set; }
            public String MiddleName { get; private set; }
            public String LastName { get; private set; }
            public String Street { get; private set; }
            public String HouseNumber { get; private set; }
            public String HouseNumberAddition { get; private set; }
            public String PostalCode { get; private set; }
            public String City { get; private set; }
            public CountryCode CountryCode { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

            /// <summary>
            /// - Must not be null
            /// - Must be a valid name
            /// - Maximum length of 50 characters
            /// </summary>
            /// <param name="firstName">First name</param>
            /// <returns>Builder</returns>
            public Builder WithFirstName(String firstName)
            {
                this.FirstName = firstName;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Should contain only alphabetic characters
            /// - Maximum length of 20 characters
            /// </summary>
            /// <param name="middleName">Middle name</param>
            /// <returns>Builder</returns>
            public Builder WithMiddleName(String middleName)
            {
                this.MiddleName = middleName;
                return this;
            }

            /// <summary>
            /// - Should not be null or empty
            /// - Should contain only alphabetic characters
            /// - Maximum length of 50 characters
            /// </summary>
            /// <param name="lastName">Last name</param>
            /// <returns>Builder</returns>
            public Builder WithLastName(String lastName)
            {
                this.LastName = lastName;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must be a valid street
            /// - Maximum length of 100 characters
            /// </summary>
            /// <param name="street">Street</param>
            /// <returns>Builder</returns>
            public Builder WithStreet(String street)
            {
                this.Street = street;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must be a valid house number
            /// - Maximum length of 100 characters
            /// </summary>
            /// <param name="houseNumber">House number</param>
            /// <returns>Builder</returns>
            public Builder WithHouseNumber(String houseNumber)
            {
                this.HouseNumber = houseNumber;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must be alphanumeric
            /// - Maximum length of 6 characters
            /// </summary>
            /// <param name="houseNumberAddition">House number addition</param>
            /// <returns>Builder</returns>
            public Builder WithHouseNumberAddition(String houseNumberAddition)
            {
                this.HouseNumberAddition = houseNumberAddition;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must be a valid postalCode for the Country
            /// - Maximum length is dependent on the countryCode: 
            /// BE = 4 -> \p{Digit}+
            /// DE = 5 -> \p{Digit}+
            /// NL = 6 -> \p{Digit}{4}\p{Alpha}2
            /// Other = 10
            /// </summary>
            /// <param name="postalCode">Postal code</param>
            /// <returns>Builder</returns>
            public Builder WithPostalCode(String postalCode)
            {
                this.PostalCode = postalCode;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must only contain alphabetic characters
            /// - Maximum length of 40 characters
            /// </summary>
            /// <param name="city">City</param>
            /// <returns>Builder</returns>
            public Builder WithCity(String city)
            {
                this.City = city;
                return this;
            }

            /// <summary>
            /// - Must not be null
            /// - Must be a valid CountryCode
            /// </summary>
            /// <param name="countryCode">Country code</param>
            /// <returns>Builder</returns>
            public Builder WithCountryCode(CountryCode countryCode)
            {
                this.CountryCode = countryCode;
                return this;
            }

            /// <summary>
            /// Initializes and returns an Address
            /// </summary>
            /// <returns>Address</returns>
            public Address Build()
            {
                return new Address(this);
            }
        }
    }
}
