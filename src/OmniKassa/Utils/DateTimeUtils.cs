using System;
using System.Globalization;
using OmniKassa.Exceptions;

namespace OmniKassa.Utils
{
    /// <summary>
    /// Utility class for conversions between DateTime and string
    /// </summary>
    public class DateTimeUtils
    {
        private static readonly String DATE_TIME_FORMATTER = "yyyy-MM-dd'T'HH:mm:ss.fffzzz";

        private DateTimeUtils()
        {
            
        }

        /// <summary>
        /// Converts a DateTime to string
        /// </summary>
        /// <param name="dateTime">DateTime value</param>
        /// <returns>DateTime string value</returns>
        public static String DateToString(DateTime dateTime)
        {
            return dateTime.ToString(DATE_TIME_FORMATTER);
        }

        /// <summary>
        /// Converts a string to DateTime
        /// </summary>
        /// <param name="date">DateTime string value</param>
        /// <returns>DateTime</returns>
        public static DateTime StringToDate(String date)
        {
            if (date == null)
            {
                return DateTime.MinValue;
            }

            try
            {
                DateTime newDate = DateTime.ParseExact(date, DATE_TIME_FORMATTER, CultureInfo.InvariantCulture);
                return newDate;
            }
            catch (Exception)
            {
                throw new RabobankSdkException("Could not convert date string to DateTime");
            }
        }
    }
}
