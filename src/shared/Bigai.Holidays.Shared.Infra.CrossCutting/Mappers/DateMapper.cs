using Bigai.Holidays.Shared.Infra.CrossCutting.Helpers;
using System;

namespace Bigai.Holidays.Shared.Infra.CrossCutting.Mappers
{
    /// <summary>
    /// <see cref="DateMapper"/> Contains methods to support converting or obtaining values from one type to another.
    /// </summary>
    public static class DateMapper
    {
        /// <summary>
        /// Converts date and time to SQL date and time.
        /// </summary>
        /// <param name="date">DateTime to be converted.</param>
        /// <returns>DateTime in SQL format.</returns>
        public static string ToSqlDate(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

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

        /// <summary>
        /// Determines the date on which Ascension Day, occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine Ascension Day.</param>
        /// <returns>Ascension Day. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetAscensionDay(this int year)
        {
            const int ascensionDay = 39;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(ascensionDay) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Ash Wednesday occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine ash wednesday.</param>
        /// <returns>Ash Wednesday. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetAshWednesday(this int year)
        {
            const int ashWednesday = -46;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(ashWednesday) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Carnival Monday occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine carnival monday.</param>
        /// <returns>Carnival Monday. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetCarnivalMonday(this int year)
        {
            const int carnivalMonday = -48;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(carnivalMonday) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Carnival Tuesday occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine carnival tuesday.</param>
        /// <returns>Carnival Tuesday. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetCarnivalTuesday(this int year)
        {
            const int carnivalTuesday = -47;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(carnivalTuesday) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Corpus Christi, Passion of Christ occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine Corpus Christi.</param>
        /// <returns>Corpus Christi. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetCorpusChristi(this int year)
        {
            const int corpusChristi = 60;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(corpusChristi) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Easter occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine easter.</param>
        /// <returns>Easter Date. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetEasterSunday(this int year)
        {
            return year.IsYear() ? year.Easter() : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Good Friday, Passion of Christ occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine good friday.</param>
        /// <returns>Good Friday. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetGoodFriday(this int year)
        {
            const int goodFriday = -2;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(goodFriday) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the day of the Holy Saturday, which happens on the eve of Easter.
        /// </summary>
        /// <param name="year">Year to determine Holy Saturday.</param>
        /// <returns></returns>
        public static DateTime GetHolySaturday(this int year)
        {
            const int holySaturday = -1;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(holySaturday) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Holy Thursday, occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine Holy Thursday.</param>
        /// <returns>Ascension Thursday. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetHolyThursday(this int year)
        {
            const int holyThursday = -3;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(holyThursday) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Maundy Thursday, occurs, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine Holy Thursday.</param>
        /// <returns>Ascension Thursday. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetMaundyThursday(this int year)
        {
            return year.GetHolyThursday();
        }

        /// <summary>
        /// Determines the date on which Pentecost Day, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine Pentecost Day.</param>
        /// <returns>Pentecost. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetPentecostDay(this int year)
        {
            const int pentecost = 49;

            DateTime easterSunday = year.Easter();

            return year.IsYear() ? easterSunday.AddDays(pentecost) : DateTime.MinValue;
        }

        /// <summary>
        /// Determines the date on which Pentecost, Whitsun Day, in a specific year.
        /// </summary>
        /// <param name="year">Year to determine Whitsun Day.</param>
        /// <returns>Whitsun. In case the year is invalid <c>DateTime.MinValue</c>.</returns>
        public static DateTime GetWhitsunDay(this int year)
        {
            return year.GetPentecostDay();
        }

        /// <summary>
        /// Determines the first work day from Monday to Friday, after a date.
        /// </summary>
        /// <param name="date">Date to be tested.</param>
        /// <returns>First work day after a date.</returns>
        public static DateTime GetFirstWorkDayAfterDate(this DateTime date)
        {
            do
            {
                date = date.AddDays(1);

            } while (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);

            return date;
        }

        /// <summary>
        /// Determines the number of times of week.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns>Number that corresponds to number of weeks.</returns>
        public static int ToWeekTimes(this string value)
        {
            if (!value.HasValue())
            {
                return 0;
            }

            int times;
            value = value.ToLower().Trim();
            try
            {
                times = value.HasValue() ? int.Parse(value.Trim()) : 0;
            }
            catch
            {
                times = 0;
            }

            return times;
        }

        /// <summary>
        /// Determines the day of week from sunday to saturday.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns>returns a number that corresponds to a day of the week. In case of error return -1.</returns>
        public static int ToDayOfWeek(this string value)
        {
            if (!value.HasValue())
            {
                return -1;
            }

            DayOfWeek? dayOfWeek = null;
            value = value.ToLower().Trim();

            if (value.StartsWith("sunday"))
            {
                dayOfWeek = DayOfWeek.Sunday;
            }
            else if (value.StartsWith("monday"))
            {
                dayOfWeek = DayOfWeek.Monday;
            }
            else if (value.StartsWith("tuesday"))
            {
                dayOfWeek = DayOfWeek.Tuesday;
            }
            else if (value.StartsWith("wednesday"))
            {
                dayOfWeek = DayOfWeek.Wednesday;
            }
            else if (value.StartsWith("thursday"))
            {
                dayOfWeek = DayOfWeek.Thursday;
            }
            else if (value.StartsWith("friday"))
            {
                dayOfWeek = DayOfWeek.Friday;
            }
            else if (value.StartsWith("saturday"))
            {
                dayOfWeek = DayOfWeek.Saturday;
            }

            return dayOfWeek == null ? -1 : (int)dayOfWeek;
        }

        /// <summary>
        /// Determines the first day of week after a date.
        /// </summary>
        /// <param name="date">Initial date to be tested.</param>
        /// <param name="dayOfWeek">Name of the day of week to get the date.</param>
        /// <returns>Date corresponding to the day of the week, after the start date.</returns>
        public static DateTime GetDayAfterDate(this DateTime date, DayOfWeek dayOfWeek)
        {
            do
            {
                date = date.AddDays(1);

            } while (date.DayOfWeek != dayOfWeek);

            return date;
        }

        /// <summary>
        /// Determines the number of the week according to the value.
        /// </summary>
        /// <param name="value">Value to be tested.</param>
        /// <returns>Number that corresponds to week from 1 to 4. In case of error return -1.</returns>
        /// <remarks>1 for the first week, 2 for the second, 3 for the third and 4 for the fourth and last week of the month.</remarks>
        public static int ToWeekNumber(this string value)
        {
            if (!value.HasValue())
            {
                return -1;
            }

            value = value.ToLower().Trim();
            int week;
            if (value == "last")
            {
                week = 4;
            }
            else
            {
                try
                {
                    week = value.HasValue() ? int.Parse(value.Trim()) : -1;
                    if (week == 0 || week > 4)
                    {
                        week = -1;
                    }
                }
                catch
                {
                    week = -1;
                }
            }

            return week;
        }

        /// <summary>
        /// Determines the date on which the day of the week takes place, in the week of the month.
        /// </summary>
        /// <param name="year">Calendar year.</param>
        /// <param name="month">Calendar month.</param>
        /// <param name="weekday">Name of the day of week.</param>
        /// <param name="weekNumber">Number of week.</param>
        /// <returns>Date for the name of the day in the requested week.</returns>
        public static DateTime GetWeekDayOfMonth(this int year, int month, DayOfWeek weekday, int weekNumber)
        {
            if (weekNumber < 1 || weekNumber > 4)
            {
                return DateTime.MinValue;
            }

            DateTime currentDate = new DateTime(year, month, 1);

            int weekdayCount = 0;

            while (weekdayCount < weekNumber)
            {
                if (currentDate.DayOfWeek == weekday)
                {
                    weekdayCount++;
                }

                if (weekdayCount < weekNumber)
                {
                    currentDate = currentDate.AddDays(1);
                }
            }

            return currentDate;
        }


        /// <summary>
        /// Determines if the year is is valid. To be considered valid it must be between 1900 and 2300.
        /// </summary>
        /// <param name="year">Year of holiday.</param>
        /// <returns>Retrurn <c>true</c> if the year is betwenn 1900 and 2300, otherwise <c>false</c>.</returns>
        private static bool IsYear(this int year)
        {
            return (year >= 1900 && year <= 2300);
        }

    }
}
