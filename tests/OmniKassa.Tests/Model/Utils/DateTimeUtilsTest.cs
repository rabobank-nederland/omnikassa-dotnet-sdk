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

        [Fact(Skip = "Fix timezone problem first")]
        public void StringToDate()
        {
            AssertDateTime(DateTimeUtils.StringToDate(DATETIME_STRING));
        }

        [Fact(Skip = "Fix timezone problem first")]
        public void StringToDate_AlsoSupportsNoColons()
        {
            AssertDateTime(DateTimeUtils.StringToDate(DATETIME_STRING_NO_COLON));
        }

        private void AssertDateTime(DateTime dateTime)
        {
            Assert.Equal(2016,  dateTime.Year);
            Assert.Equal(7,     dateTime.Month);
            Assert.Equal(28,    dateTime.Day);
            Assert.Equal(12,    dateTime.Hour);
            Assert.Equal(58,    dateTime.Minute);
            Assert.Equal(50,    dateTime.Second);
            Assert.Equal(205,   dateTime.Millisecond);
        }

        [Fact]
        public void StringToDateInValidFormat()
        {
            Assert.Throws<RabobankSdkException>(() => DateTimeUtils.StringToDate("07-28T12:58:50.205+0200"));
        }

        [Fact(Skip = "Fix timezone problem first")]
        public void DateToString_Should_ReturnStringWithRightDate_When_DateObjectIsValid()
        {
            DateTime dateTime = new DateTime(2016, 7, 28, 12, 58, 50, 205);
            Assert.Equal(DATETIME_STRING, DateTimeUtils.DateToString(dateTime));
        }

        [Fact(Skip = "Fix timezone problem first")]
        public void StringToDateThenDateToString_Should_ReturnOriginalString_When_Always()
        {
            DateTime dateTime = DateTimeUtils.StringToDate(DATETIME_STRING);
            String calendarString = DateTimeUtils.DateToString(dateTime);
            Assert.Equal(DATETIME_STRING, calendarString);
        }

    }
}
