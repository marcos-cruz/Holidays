using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays
{
    /// <summary>
    /// <see cref="IAddRuleHolidayService"/> represents a contract to validate business rules and add <see cref="RuleHoliday"/> of holidays to the database.
    /// </summary>
    public interface IAddRuleHolidayService : IDomainService
    {
        /// <summary>
        /// Add a <see cref="RuleHoliday"/> in database.
        /// </summary>
        /// <param name="ruleHoliday">Instance of <see cref="RuleHoliday"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="RuleHoliday"/> added to the database.</returns>
        CommandResult Add(RuleHoliday ruleHoliday);

        /// <summary>
        /// Add a <see cref="RuleHoliday"/> in database.
        /// </summary>
        /// <param name="ruleHoliday">Instance of <see cref="RuleHoliday"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="RuleHoliday"/> added to the database.</returns>
        Task<CommandResult> AddAsync(RuleHoliday ruleHoliday);

        /// <summary>
        /// Add a list of <see cref="RuleHoliday"/> in database.
        /// </summary>
        /// <param name="listOfRuleHolidays">List of <see cref="RuleHoliday"/> to be added.</param>
        CommandResult AddRange(List<RuleHoliday> listOfRuleHolidays);

        /// <summary>
        /// Add a list of <see cref="RuleHoliday"/> in database.
        /// </summary>
        /// <param name="listOfRuleHolidays">List of <see cref="RuleHoliday"/> to be added.</param>
        Task<CommandResult> AddRangeAsync(List<RuleHoliday> listOfRuleHolidays);

        /// <summary>
        /// Add a list of <see cref="RuleHoliday"/> lists in database.
        /// </summary>
        /// <param name="listOfListRuleHolidays">List of <see cref="RuleHoliday"/> lists to be added.</param>
        CommandResult AddRange(List<List<RuleHoliday>> listOfListRuleHolidays);

        /// <summary>
        /// Add a list of <see cref="RuleHoliday"/> lists in database.
        /// </summary>
        /// <param name="listOfListRuleHolidays">List of <see cref="RuleHoliday"/> lists to be added.</param>
        Task<CommandResult> AddRangeAsync(List<List<RuleHoliday>> listOfListRuleHolidays);
    }
}
