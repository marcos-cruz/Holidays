using System;

namespace Bigai.Holidays.Core.Domain.Responses
{
    /// <summary>
    /// Detail of a holiday
    /// </summary>
    public class HolidayResponse
    {
        /// <summary>
        /// Date on which the holiday occurs.
        /// </summary>
        public DateTime HolidayDate { get; set; }

        /// <summary>
        /// Name of the day on which the holiday takes place, for example, Wednesday.
        /// </summary>
        public string HolidayDay { get; set; }

        /// <summary>
        /// Type of holiday, which can be: <c>City</c>, <c>National</c>, and <c>State</c>.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Description of the holiday in the native language, for example, in Brazilian Portuguese.
        /// </summary>
        public string NativeDescription { get; set; }

        /// <summary>
        /// Description of the holiday in an alternative language, for example, in English.
        /// </summary>
        public string AlternativeDescription { get; set; }

        /// <summary>
        /// Country code, consisting of 3 letters, according to ISO-3166.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// State abbreviation, consisting of 2 letters, according to ISO-3166.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Code with up to 32 characters, which identifies the city.
        /// </summary>
        public string CityCode { get; set; }
    }
}
