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
        /// Add a <see cref="Holiday"/> in database.
        /// </summary>
        /// <param name="holiday">Instance of <see cref="Holiday"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="Holiday"/> added to the database.</returns>
        CommandResult Add(Holiday holiday);

        /// <summary>
        /// Add a <see cref="Holiday"/> in database.
        /// </summary>
        /// <param name="holiday">Instance of <see cref="Holiday"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="Holiday"/> added to the database.</returns>
        Task<CommandResult> AddAsync(Holiday holiday);

        /// <summary>
        /// Add a list of <see cref="Holiday"/> in database.
        /// </summary>
        /// <param name="listOfHolidays">List of <see cref="Holiday"/> to be added.</param>
        CommandResult AddRange(List<Holiday> listOfHolidays);

        /// <summary>
        /// Add a list of <see cref="Holiday"/> in database.
        /// </summary>
        /// <param name="listOfHolidays">List of <see cref="Holiday"/> to be added.</param>
        Task<CommandResult> AddRangeAsync(List<Holiday> listOfHolidays);

        /// <summary>
        /// Add a list of <see cref="Holiday"/> lists in database.
        /// </summary>
        /// <param name="listOfListHolidays">List of <see cref="Holiday"/> lists to be added.</param>
        CommandResult AddRange(List<List<Holiday>> listOfListHolidays);

        /// <summary>
        /// Add a list of <see cref="Holiday"/> lists in database.
        /// </summary>
        /// <param name="listOfListHolidays">List of <see cref="Holiday"/> lists to be added.</param>
        Task<CommandResult> AddRangeAsync(List<List<Holiday>> listOfListHolidays);
    }
}
