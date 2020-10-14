using System;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Helpers
{
    /// <summary>
    /// <see cref="DateHelper"/> extension methods that help with date validation.
    /// </summary>
    public static class DateHelper
    {
        /// <summary>
        /// Determines if the year is is valid. To be considered valid it must be between 1900 and 2300.
        /// </summary>
        /// <param name="year">Year of holiday.</param>
        /// <returns>Retrurn <c>true</c> if the year is betwenn 1900 and 2300, otherwise <c>false</c>.</returns>
        public static bool IsYear(this int year)
        {
            return (year >= 1900 && year <= 2300);
        }

        /// <summary>
        /// Converts a value in dd-mm-yyyy format to a date
        /// </summary>
        /// <param name="value">Value to to converted to date.</param>
        /// <returns>Value converted to data.In case of error <c>null</c>.</returns>
        public static DateTime? ToDate(this string value)
        {
            DateTime? date = null;

            if (!value.HasValue())
            {
                return null;
            }

            value = value.Replace("-", "");
            if (value.Length != 8)
            {
                return null;
            }

            int day = int.Parse(value.Substring(0, 2));
            int month = int.Parse(value.Substring(2, 2));
            int year = int.Parse(value.Substring(3));

            try
            {
                date = new DateTime(year, month, day);
            }
            catch (Exception) { }

            return date;
        }
    }
}
