using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Queries.Holidays
{
    /// <summary>
    /// <see cref="IQueryHolidaysByState"/> represents a contract for searching a state's holidays for a specific month.
    /// </summary>
    public interface IQueryHolidaysByState
    {
        /// <summary>
        /// Gets a list of all national and state holidays for a specific state and year.
        /// </summary>
        /// <param name="request">Query parameters.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code.</returns>
        Task<CommandResult> GetHolidaysByStateAsync(GetHolidaysByStateRequest request);
    }
}
