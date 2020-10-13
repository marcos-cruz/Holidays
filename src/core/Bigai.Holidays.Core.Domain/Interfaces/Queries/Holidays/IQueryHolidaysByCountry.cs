using Bigai.Holidays.Core.Domain.Requests.Holidays;
using Bigai.Holidays.Shared.Domain.Commands;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Queries.Holidays
{
    public interface IQueryHolidaysByCountry
    {
        /// <summary>
        /// Gets gets a list of all holidays for a specific country and year.
        /// </summary>
        /// <param name="request">Query parameters.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code.</returns>
        Task<CommandResult> GetHolidaysByCountryAsync(GetHolidaysByCountryRequest request);
    }
}
