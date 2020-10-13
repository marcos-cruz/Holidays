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
    }
}
