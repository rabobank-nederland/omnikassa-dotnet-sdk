using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Order
{
    /// <summary>
    /// Customer information
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CustomerInformation
    {
        /// <summary>
        /// Email address
        /// </summary>
        [JsonProperty(PropertyName = "emailAddress")]
        public String EmailAddress { get; private set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        [JsonProperty(PropertyName = "dateOfBirth")]
        public String DateOfBirth { get; private set; }

        /// <summary>
        /// Initials
        /// </summary>
        [JsonProperty(PropertyName = "initials")]
        public String Initials { get; private set; }

        /// <summary>
        /// Telephone number
        /// </summary>
        [JsonProperty(PropertyName = "telephoneNumber")]
        public String TelephoneNumber { get; private set; }

        /// <summary>
        /// Gender
        /// </summary>
        [JsonProperty(PropertyName = "gender")]
        [JsonConverter(typeof(EnumJsonConverter<Gender>))]
        public Gender? Gender { get; private set; }

        /// <summary>
        /// Initializes an empty CustomerInformation
        /// </summary>
        public CustomerInformation()
        {
            
        }

        /// <summary>
        /// Initializes an CustomerInformation using the Builder
        /// </summary>
        /// <param name="builder">Builder</param>
        public CustomerInformation(Builder builder)
        {
            EmailAddress = builder.EmailAddress;
            DateOfBirth = builder.DateOfBirth;
            Gender = builder.Gender;
            Initials = builder.Initials;
            TelephoneNumber = builder.TelephoneNumber;
        }

        private String GetNullSafe(Enum value)
        {
            if (value == null)
            {
                return null;
            }
            return value.ToString();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="o">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override Boolean Equals(Object o)
        {
            if (this == o)
            {
                return true;
            }
            if (!(o is CustomerInformation)) {
                return false;
            }
            CustomerInformation that = (CustomerInformation)o;
            return Equals(EmailAddress, that.EmailAddress) &&
                   Equals(DateOfBirth, that.DateOfBirth) &&
                   Equals(Initials, that.Initials) &&
                   Equals(TelephoneNumber, that.TelephoneNumber) &&
                   Gender == that.Gender;
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
                hash = (hash * -1521134295) + (EmailAddress == null ? 0 : EmailAddress.GetHashCode());
                hash = (hash * -1521134295) + (DateOfBirth == null ? 0 : DateOfBirth.GetHashCode());
                hash = (hash * -1521134295) + (Initials == null ? 0 : Initials.GetHashCode());
                hash = (hash * -1521134295) + (TelephoneNumber == null ? 0 : TelephoneNumber.GetHashCode());
                hash = (hash * -1521134295) + (Gender == null ? 0 : Gender.GetHashCode());
                return hash;
            }
        }

        /// <summary>
        /// CustomerInformation builder
        /// </summary>
        public class Builder
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public String EmailAddress { get; private set; }
            public Gender? Gender { get; private set; }
            public String Initials { get; private set; }
            public String TelephoneNumber { get; private set; }
            public String DateOfBirth { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

            /// <summary>
            /// - Optional
            /// - Must be a valid email address
            /// - Maximum length of 45 characters
            /// </summary>
            /// <param name="emailAddress">Email address</param>
            /// <returns>Builder</returns>
            public Builder WithEmailAddress(String emailAddress)
            {
                this.EmailAddress = emailAddress;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must be one of the <see cref="Gender"/> values
            /// </summary>
            /// <param name="gender">Gender</param>
            /// <returns>Builder</returns>
            public Builder WithGender(Gender? gender)
            {
                this.Gender = gender;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must only contain alphabetic characters
            /// - Maximum length of 256 characters
            /// </summary>
            /// <param name="initials">Initials</param>
            /// <returns>Builder</returns>
            public Builder WithInitials(String initials)
            {
                this.Initials = initials;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must be a valid telephone number
            /// - Must only contain alphanumeric characters
            /// - Maximum length of 31 characters
            /// </summary>
            /// <param name="telephoneNumber">Telephone number</param>
            /// <returns>Builder</returns>
            public Builder WithTelephoneNumber(String telephoneNumber)
            {
                this.TelephoneNumber = telephoneNumber;
                return this;
            }

            /// <summary>
            /// - Optional
            /// - Must be a valid birth date
            /// - Only the DD-MM-YYYY format is accepted
            /// </summary>
            /// <param name="dateOfBirth">Date of birth</param>
            /// <returns>Builder</returns>
            public Builder WithDateOfBirth(String dateOfBirth)
            {
                this.DateOfBirth = dateOfBirth;
                return this;
            }

            /// <summary>
            /// Initializes and returns a CustomerInformation
            /// </summary>
            /// <returns>CustomerInformation</returns>
            public CustomerInformation Build()
            {
                return new CustomerInformation(this);
            }
        }
    }
}
