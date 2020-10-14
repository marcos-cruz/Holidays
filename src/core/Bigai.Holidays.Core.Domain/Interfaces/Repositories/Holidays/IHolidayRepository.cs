using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays
{
    /// <summary>
    /// <see cref="IHolidayRepository"/> represents a contract to persist <see cref="Holiday"/> in relational database.
    /// </summary>
    public interface IHolidayRepository : IRepository<Holiday>
    {
        /// <summary>
        /// Gets an lists of <see cref="Holiday"/> by country and year.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="year">Year for holidays filter.</param>
        /// <returns></returns>
        Task<IEnumerable<Holiday>> GetHolidaysByCountryAsync(string countryIsoCode, int year);

        /// <summary>
        /// Gets an lists of <see cref="Holiday"/> by country and date.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="holidayDate">Date for holidays filter.</param>
        /// <returns></returns>
        Task<IEnumerable<Holiday>> GetHolidaysByDateAsync(string countryIsoCode, DateTime holidayDate);

        /// <summary>
        /// Get a list of a country's holidays in a specific month.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="year">Year for holidays filter.</param>
        /// <param name="year">Month for holidays filter.</param>
        /// <returns>List of a country's holidays in a specific month.</returns>
        Task<IEnumerable<Holiday>> GetHolidaysByMonthAsync(string countryIsoCode, int year, int month);

        /// <summary>
        /// Get a list of a states's holidays in a specific year.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="stateIsoCode">State code according to ISO-3166.</param>
        /// <param name="year">Month for holidays filter.</param>
        /// <returns>List of state holidays in the year.</returns>
        Task<IEnumerable<Holiday>> GetHolidaysByStateAsync(string countryIsoCode, string stateIsoCode, int year);
    }
}
