using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays
{
    /// <summary>
    /// <see cref="IAddHolidayService"/> represents a contract to validate business rules and add <see cref="Holiday"/> of holidays to the database.
    /// </summary>
    public interface IAddHolidayService : IDomainService
    {
        /// <summary>
        /// Adds holidays to the database for a specific country and year.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="year">Year for holidays filter.</param>
        /// <returns>Returns an instance of XPTO containing the operation status code.</returns>
        Task<CommandResult> AddAsync(string countryIsoCode, int year);

        /// <summary>
        /// Add a <see cref="Holiday"/> in database.
        /// </summary>
        /// <param name="holiday">Instance of <see cref="Holiday"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code and the <see cref="Holiday"/> added to the database.</returns>
        Task<CommandResult> AddAsync(Holiday holiday);

        /// <summary>
        /// Add a list of <see cref="Holiday"/> in database.
        /// </summary>
        /// <param name="listOfHolidays">List of <see cref="Holiday"/> to be added.</param>
        /// <param name="validateRepository">Determines whether the list should be validated. Default <c>true</c>.</param>
        Task<CommandResult> AddRangeAsync(List<Holiday> listOfHolidays, bool validateRepository = true);

        /// <summary>
        /// Add a list of <see cref="Holiday"/> lists in database.
        /// </summary>
        /// <param name="listOfListHolidays">List of <see cref="Holiday"/> lists to be added.</param>
        /// <param name="validateRepository">Determines whether the list should be validated. Default <c>true</c>.</param>
        Task<CommandResult> AddRangeAsync(List<List<Holiday>> listOfListHolidays, bool validateRepository = true);
    }
}
