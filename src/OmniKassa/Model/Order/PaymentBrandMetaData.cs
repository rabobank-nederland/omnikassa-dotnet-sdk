using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model.Order
{
    /// <summary>
    /// Additional metadata specific to the brand as set in the paymentBrand field.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentBrandMetaData
    {
        /// <summary>
        /// Use this field to immediately start an iDEAL transaction at a specific issuer. 
        /// To do so set the paymentBrand field to 'IDEAL' and set this field to the BIC code of the iDEAL issuer. 
        /// If this field is not included in the request then Rabo Smart Pay will first present a list of available issuers to the consumer before starting a transaction.
        /// </summary>
        [JsonProperty(PropertyName = "issuerId", NullValueHandling = NullValueHandling.Ignore)]
        public String IssuerId { get; set; }

        /// <summary>
        /// Contains fields related to fast checkout flow. When this is defined, it activates the fast checkout flow.
        /// When fast checkout is defined:
        /// - paymentBrandForce must be specified and set to FORCE_ALWAYS.
        /// - specified paymentBrand must be set to 'IDEAL'
        /// </summary>
        [JsonProperty(PropertyName = "fastCheckout", NullValueHandling = NullValueHandling.Ignore)]
        public FastCheckout FastCheckout { get; set; }

        /// <summary>
        /// Whether a card on file should be used.
        /// </summary>
        [JsonProperty(PropertyName = "enableCardOnFile", NullValueHandling = NullValueHandling.Ignore)]
        public Boolean? EnableCardOnFile;

        /// <summary>
        /// Initializes an empty PaymentBrandMetaData
        /// </summary>
        public PaymentBrandMetaData()
        {
        }

        /// <summary>
        /// Initializes a PaymentBrandMetaData using the Builder
        /// </summary>
        /// <param name="builder">Builder</param>
        private PaymentBrandMetaData(Builder builder)
        {
            IssuerId = builder.IssuerId;
            FastCheckout = builder.FastCheckout;
            EnableCardOnFile = builder.EnableCardOnFile;
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
            if (!(obj is PaymentBrandMetaData))
            {
                return false;
            }
            var other = (PaymentBrandMetaData)obj;

            return Equals(IssuerId, other.IssuerId) &&
                   EqualsPaymentBrandMetaData(FastCheckout?.RequiredCheckoutFields, other.FastCheckout?.RequiredCheckoutFields) &&
                   Equals(EnableCardOnFile, other.EnableCardOnFile);
        }
        
        private bool EqualsPaymentBrandMetaData(
            IReadOnlyList<RequiredCheckoutFields> one,
            IReadOnlyList<RequiredCheckoutFields> two)
        {
            // Handle empty dictionaries as null
            var tmpOne = one != null && one.Count > 0 ? one : null;
            var tmpTwo = two != null && two.Count > 0 ? two : null;

            // When both are null, items are the same.
            if (tmpOne == null && tmpTwo == null)
            {
                return true;
            }
            // If only one is null, items are not the same.
            if (tmpOne == null || tmpTwo == null)
            {
                return false;
            }
            
            if (one.Count != two.Count)
            {
                return false;
            }

            return one.All(requiredCheckoutField => two.Contains(requiredCheckoutField));
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
                hash = (hash * -1521134295) + (IssuerId == null ? 0 : IssuerId.GetHashCode());
                hash = (hash * -1521134295) + (FastCheckout == null ? 0 : FastCheckout.GetHashCode());
                hash = (hash * -1521134295) + (EnableCardOnFile == null ? 0 : EnableCardOnFile.GetHashCode());
                return hash;
            }
        }

        /// <summary>
        /// PaymentBrandMetaData builder
        /// </summary>
        public class Builder
        {
            /// <summary>
            /// Ideal issuer ID.
            /// </summary>
            public String IssuerId { get; private set; }
            
            /// <summary>
            /// Fast checkout configuration.
            /// </summary>
            public FastCheckout FastCheckout { get; private set; }

            /// <summary>
            /// Use card on file.
            /// </summary>
            public Boolean EnableCardOnFile { get; private set; }

            /// <summary>
            /// Sets the issuer ID for iDEAL transactions.
            /// Use this field to immediately start an iDEAL transaction at a specific issuer.
            /// Set this field to the BIC code of the iDEAL issuer.
            /// </summary>
            /// <param name="issuerId">BIC code of the iDEAL issuer</param>
            /// <returns>Builder</returns>
            public Builder WithIssuerId(String issuerId)
            {
                IssuerId = issuerId;
                return this;
            }

            /// <summary>
            /// Sets the fast checkout configuration.
            /// When fast checkout is defined:
            /// - paymentBrandForce must be specified and set to FORCE_ALWAYS.
            /// - specified paymentBrand must be set to 'IDEAL'
            /// </summary>
            /// <param name="fastCheckout">Fast checkout configuration</param>
            /// <returns>Builder</returns>
            public Builder WithFastCheckout(FastCheckout fastCheckout)
            {
                FastCheckout = fastCheckout;
                return this;
            }

            /// <summary>
            /// Setsh the use card on file property.
            /// </summary>
            /// <param name="enableCardOnFile">The cards on file property</param>
            /// <returns>Builder</returns>
            public Builder WithEnableCardOnFile(Boolean enableCardOnFile)
            {
                EnableCardOnFile = enableCardOnFile;
                return this;
            }

            /// <summary>
            /// Initializes and returns a PaymentBrandMetaData
            /// </summary>
            /// <returns>PaymentBrandMetaData</returns>
            public PaymentBrandMetaData Build()
            {
                return new PaymentBrandMetaData(this);
            }
        }
    }
}
