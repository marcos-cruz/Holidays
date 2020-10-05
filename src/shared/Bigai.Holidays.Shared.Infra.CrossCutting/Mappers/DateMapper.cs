using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Mappers
{
    /// <summary>
    /// <see cref="DateMapper"/> Contains methods to support converting or obtaining values from one type to another.
    /// </summary>
    public static class DateMapper
    {
        /// <summary>
        /// Determines the number of the month.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns>Return the number of the month. If the value does not correspond to a month, then returns -1.</returns>
        public static int ToMonth(this string value)
        {
            if (!value.HasValue())
            {
                return -1;
            }

            int month = -1;
            value = value.ToLower().Trim();

            if (value.StartsWith("jan") || value == "1")
            {
                month = 1;
            }
            else if (value.StartsWith("feb") || value == "2")
            {
                month = 2;
            }
            else if (value.StartsWith("mar") || value == "3")
            {
                month = 3;
            }
            else if (value.StartsWith("apr") || value == "4")
            {
                month = 4;
            }
            else if (value.StartsWith("may") || value == "5")
            {
                month = 5;
            }
            else if (value.StartsWith("jun") || value == "6")
            {
                month = 6;
            }
            else if (value.StartsWith("jul") || value == "7")
            {
                month = 7;
            }
            else if (value.StartsWith("aug") || value == "8")
            {
                month = 8;
            }
            else if (value.StartsWith("sep") || value == "9")
            {
                month = 9;
            }
            else if (value.StartsWith("oct") || value == "10")
            {
                month = 10;
            }
            else if (value.StartsWith("nov") || value == "11")
            {
                month = 11;
            }
            else if (value.StartsWith("dec") || value == "12")
            {
                month = 12;
            }

            return month;
        }

        /// <summary>
        /// Determines the number of the day from 1 to 31.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns>Number that corresponds to the day from 1 to 31. In case of error return -1.</returns>
        public static int ToDay(this string value)
        {
            if (!value.HasValue())
            {
                return -1;
            }

            int day;
            value = value.Trim();
            try
            {
                day = value.HasValue() ? int.Parse(value.Trim()) : -1;
                if (day < 1 || day > 31)
                {
                    day = -1;
                }
            }
            catch
            {
                day = -1;
            }

            return day;
        }
    }
}
