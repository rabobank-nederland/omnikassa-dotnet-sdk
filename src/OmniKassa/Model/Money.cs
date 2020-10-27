using System;
using Newtonsoft.Json;
using OmniKassa.Model.Enums;

namespace OmniKassa.Model
{
    /// <summary>
    /// Represents money. The amount is stored as a <see cref="Decimal"/>, and is limited by two decimal places.
    /// Money is stored as euros, 1 EUR is;<code>Money oneEuro = new Money(Currency.EUR, 1.00m);</code>
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Money
    {
        /// <summary>
        /// Currency
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        [JsonConverter(typeof(EnumJsonConverter<Currency>))]
        public Currency Currency { get; private set; }

        /// <summary>
        /// Money amount
        /// </summary>
        public Decimal Amount { get; set; }

        [JsonProperty(PropertyName = "amount")]
        private String AmountInCents
        {
            get
            {
                return Convert.ToString(GetAmountInCents());
            }
            set
            {
                Amount = ParseAmount(value);
            }
        }

        private Money()
        {

        }

        private Money(Currency currency, Decimal amount)
        {
            this.Currency = currency;
            this.Amount = amount;
        }

        /// <summary>
        /// Initializes Money by given currency and amount
        /// </summary>
        /// <param name="currency">
        /// - Must not be null
        /// - Must be a valid Currency
        /// </param>
        /// <param name="amount">
        /// - Must not be null
        /// </param>
        /// <returns>Money</returns>
        public static Money FromDecimal(Currency currency, Decimal amount)
        {
            CheckAmount(amount);
            return new Money(currency, amount);
        }

        /// <summary>
        /// Deprecated. See <see cref="FromDecimal(Currency, decimal)"/>
        /// </summary>
        [Obsolete("FromEuros is deprecated, please use FromDecimal instead.", false)]
        public static Money FromEuros(Currency currency, Decimal amount)
        {
            return FromDecimal(currency, amount);
        }

#if DEBUG
        /// <summary>
        /// Checks the amount string. Throws exception on error.
        /// </summary>
        /// <param name="amountString">Amount</param>
        public
#else
        private
#endif
        static void CheckAmountString(String amountString)
        {
            bool isValid = !String.IsNullOrEmpty(amountString);
            if (isValid)
            {
                for (int i = 0; i < amountString.Length; i++)
                {
                    char c = amountString[i];
                    // amountString is only valid if;
                    // - All characters are digits, or
                    // - The first character is a minus sign, but then the String must be at least two
                    // characters or longer
                    if (!(Char.IsDigit(c) || (i == 0 && c == '-' && amountString.Length > 1)))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            if(!isValid)
            {
                throw new ArgumentException("Amount must be in cents, and must be a valid number");
            }
        }

        private static void CheckAmount(Decimal amount)
        {
            if (GetNumberOfDecimalPlaces(amount) > 2)
            {
                throw new ArgumentException("Amount must have at most 2 decimal places, and must be a valid number");
            }
        }

        private static int GetNumberOfDecimalPlaces(Decimal value)
        {
            Decimal value2 = Math.Round(value, 2);
            return Math.Max(0, value.ToString().Length - (value2.ToString().Length - 2));
        }

        private static Decimal ParseAmount(String amountString)
        {
            CheckAmountString(amountString);
            Decimal amount = Convert.ToDecimal(amountString) * 0.01m;
            CheckAmount(amount);
            return amount;
        }

        /// <summary>
        /// Gets the money amount in cents
        /// </summary>
        /// <returns>Amount in cents</returns>
        public long GetAmountInCents()
        {
            return Convert.ToInt64(Amount * 100);
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
            if (!(o is Money))
            {
                return false;
            }
            Money that = (Money)o;
            return Equals(Currency, that.Currency) &&
                   Equals(Amount, that.Amount) &&
                   Equals(AmountInCents, that.AmountInCents);
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
                hash = (hash * -1521134295) + Currency.GetHashCode();
                hash = (hash * -1521134295) + Amount.GetHashCode();
                hash = (hash * -1521134295) + (AmountInCents == null ? 0 : AmountInCents.GetHashCode());
                return hash;
            }
        }
    }
}
