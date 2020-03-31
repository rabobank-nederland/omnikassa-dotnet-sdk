using System;
using Newtonsoft.Json;
using OmniKassa.Model;
using OmniKassa.Model.Enums;
using Xunit;

namespace OmniKassa.Tests.Model.Order
{
    public class MoneyTest
    {
        [Fact]
        public void TestEquals1()
        {
            Money money1 = Money.FromDecimal(Currency.EUR, 4.95m);
            Money money2 = Money.FromDecimal(Currency.EUR, 4.95m);

            Assert.True(money1.Equals(money2));
            Assert.True(money1.GetHashCode() == money2.GetHashCode());
        }

        [Fact]
        public void TestEquals2()
        {
            Money money1 = Money.FromDecimal(Currency.EUR, 4.95m);
            money1.Amount = 1.00m;
            Money money2 = Money.FromDecimal(Currency.EUR, 1.00m);

            Assert.True(money1.Equals(money2));
            Assert.True(money1.GetHashCode() == money2.GetHashCode());
        }

        [Fact]
        public void TestEquals3()
        {
            Money money1 = Money.FromDecimal(Currency.EUR, 4.95m);
            money1.Amount = 2.00m;
            Money money2 = Money.FromDecimal(Currency.EUR, 2.95m);
            money2.Amount = 2.00m;

            Assert.True(money1.Equals(money2));
            Assert.True(money1.GetHashCode() == money2.GetHashCode());
        }

        [Fact]
        public void TestEquals4()
        {
            Money money1 = Money.FromDecimal(Currency.EUR, -4.95m);
            Money money2 = Money.FromDecimal(Currency.EUR, -4.95m);

            Assert.True(money1.Equals(money2));
            Assert.True(money1.GetHashCode() == money2.GetHashCode());
        }

        [Fact]
        public void TestEquals5()
        {
            Money money1 = Money.FromDecimal(Currency.EUR, 4.95m);
            money1.Amount -= 10.00m;
            Money money2 = Money.FromDecimal(Currency.EUR, -5.05m);

            Assert.True(money1.Equals(money2));
            Assert.True(money1.GetHashCode() == money2.GetHashCode());
        }

#if DEBUG
        [Fact]
        public void CheckAmountString()
        {
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString(null));
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString(""));
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString(" "));
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString("-"));
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString("1-"));
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString("1.00"));
            Assert.Throws<ArgumentException>(() => Money.CheckAmountString("100-"));

            Money.CheckAmountString("100");
            Money.CheckAmountString("-100");
        }
#else
    #warning("Use '--configuration DEBUG' to execute test CheckAmountString() as this function is not exposed in release.")
#endif

        [Fact]
        public void FromDecimal()
        {
            Money money = Money.FromDecimal(Currency.EUR, 4.95m);

            Assert.Equal(495L, money.GetAmountInCents());
            Assert.Equal(Currency.EUR, money.Currency);
            Assert.Equal(4.95m, money.Amount);
        }

        [Fact]
        public void FromDecimal_TooMuchDecimals1()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(Currency.EUR, 1.234m));
        }

        [Fact]
        public void FromDecimal_TooMuchDecimals2()
        {
            Assert.Throws<ArgumentException>(() => Money.FromDecimal(Currency.EUR, -1.234m));
        }

        [Fact]
        public void JsonConstructor()
        {
            Money money = GetValidMoney();

            Assert.Equal(100L, money.GetAmountInCents());
            Assert.Equal(Currency.EUR, money.Currency);
            Assert.Equal(1.00m, money.Amount);
        }

        [Fact]
        public void JsonConstructor_Should_ThrowException_When_AmountIsNotInCents()
        {
            Assert.Throws<JsonSerializationException>(() => GetInvalidMoney1());
            Assert.Throws<JsonSerializationException>(() => GetInvalidMoney2());
        }

        [Fact]
        public void Json_Should_ReturnCorrectJsonObject()
        {
            Money money = Money.FromDecimal(Currency.EUR, 1.00m);

            Assert.Equal(money, GetValidMoney());
        }

        private Money GetValidMoney()
        {
            return JsonConvert.DeserializeObject<Money>("{'amount':'100','currency':'EUR'}");
        }

        private Money GetInvalidMoney1()
        {
            return JsonConvert.DeserializeObject<Money>("{currency:'EUR',amount:'59.9'}");
        }

        private Money GetInvalidMoney2()
        {
            return JsonConvert.DeserializeObject<Money>("{currency:'EUR',amount:'59,9'}");
        }
    }
}
