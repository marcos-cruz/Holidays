using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Queries.Holidays
{
    /// <summary>
    /// <see cref="IQueryHolidaysByMonth"/> represents a contract for searching a country's holidays for a specific month.
    /// </summary>
    public interface IQueryHolidaysByMonth
    {
        /// <summary>
        /// Get a list of all holidays of the month for a specific country.
        /// </summary>
        /// <param name="request">Query parameters.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code.</returns>
        Task<CommandResult> GetHolidaysByMonthAsync(GetHolidaysByMonthRequest request);
    }
}
