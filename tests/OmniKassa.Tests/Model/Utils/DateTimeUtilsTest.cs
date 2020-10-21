using System;
using OmniKassa.Exceptions;
using OmniKassa.Utils;
using Xunit;

namespace OmniKassa.Tests.Model.Utils
{
    public class DateTimeUtilsTest
    {
        private static readonly String DATETIME_STRING = "2016-07-28T12:58:50.205+" + TestHelper.GetLocalTimeZone("\\:");
        private static readonly String DATETIME_STRING_NO_COLON = "2016-07-28T12:58:50.205+" + TestHelper.GetLocalTimeZone("");

        [Fact]
        public void StringToDate()
        {
            AssertDateTime(DateTimeUtils.StringToDate(DATETIME_STRING));
        }

        [Fact]
        public void StringToDate_AlsoSupportsNoColons()
        {
            AssertDateTime(DateTimeUtils.StringToDate(DATETIME_STRING_NO_COLON));
        }

        private void AssertDateTime(DateTime dateTime)
        {
            DateTime dateTimeInUtc = dateTime.ToUniversalTime();
            Assert.Equal(2016, dateTimeInUtc.Year);
            Assert.Equal(7, dateTimeInUtc.Month);
            Assert.Equal(28, dateTimeInUtc.Day);
            Assert.Equal(10, dateTimeInUtc.Hour);
            Assert.Equal(58, dateTimeInUtc.Minute);
            Assert.Equal(50, dateTimeInUtc.Second);
            Assert.Equal(205, dateTimeInUtc.Millisecond);
        }

        [Fact]
        public void StringToDateInValidFormat()
        {
            Assert.Throws<RabobankSdkException>(() => DateTimeUtils.StringToDate("07-28T12:58:50.205+0200"));
        }

        [Fact]
        public void DateToString_Should_ReturnStringWithRightDate_When_DateObjectIsValid()
        {
            DateTime dateTime = new DateTime(2016, 7, 28, 12, 58, 50, 205);
            String actual = DateTimeUtils.DateToString(dateTime);
            AssertDateTime("2016-07-28T12:58:50.205+", actual);
        }

        [Fact]
        public void StringToDateThenDateToString_Should_ReturnOriginalString_When_Always()
        {
            DateTime dateTime = DateTimeUtils.StringToDate(DATETIME_STRING).ToUniversalTime();
            String calendarString = DateTimeUtils.DateToString(dateTime);
            AssertDateTime("2016-07-28T10:58:50.205+", calendarString);
        }

        private void AssertDateTime(String expectedPrefix, String actual)
        {
            Assert.StartsWith(expectedPrefix, actual);
            Assert.Matches("\\d\\d:\\d\\d", actual.Substring(actual.Length - 5));
        }

    }
}
