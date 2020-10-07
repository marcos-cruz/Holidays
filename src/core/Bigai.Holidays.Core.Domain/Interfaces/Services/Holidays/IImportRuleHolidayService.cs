using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays
{
    /// <summary>
    /// <see cref="IImportRuleHolidayService"/> represents a contract to perform domain operations with CSV files for <see cref="Models.RuleHoliday"/>.
    /// </summary>
    public interface IImportRuleHolidayService : IDomainService
    {
        /// <summary>
        /// This method imports rules of holidays from a csv file and returns a list of <see cref="Models.RuleHoliday"/> lists.
        /// </summary>
        /// <param name="fileName">Name of csv file to import.</param>
        /// <returns><see cref="CommandResult"/> containing a list of <see cref="Models.RuleHoliday"/> lists in the data property.</returns>
        CommandResult Import(string userId);

        /// <summary>
        /// This method imports rules of holidays from a csv file and returns a list of <see cref="Models.RuleHoliday"/> lists.
        /// </summary>
        /// <param name="fileName">Name of csv file to import.</param>
        /// <returns><see cref="CommandResult"/> containing a list of <see cref="Models.RuleHoliday"/> lists in the data property.</returns>
        Task<CommandResult> ImportAsync(string userId);
    }
}
