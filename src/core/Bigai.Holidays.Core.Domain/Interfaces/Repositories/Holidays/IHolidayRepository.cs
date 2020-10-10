﻿using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;
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
        Task<IEnumerable<Holiday>> GetHolidaysAsync(string countryIsoCode, int year);

        /// <summary>
        /// Gets an lists of <see cref="Holiday"/> by country and year.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="year">Year for holidays filter.</param>
        /// <returns></returns>
        IEnumerable<Holiday> GetHolidays(string countryIsoCode, int year);
    }
}
