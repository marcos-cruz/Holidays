using Bigai.Holidays.Shared.Domain.Commands;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.Holidays
{
    /// <summary>
    /// <see cref="IQueryHolidayService"/> represents a contract to business rules for <see cref="Holiday"/>.
    /// </summary>
    public interface IQueryHolidayService
    {
        /// <summary>
        /// Gets an lists of <see cref="Holiday"/> by country and year.
        /// </summary>
        /// <param name="countryIsoCode">Country code consisting of 3 letters.</param>
        /// <param name="year">Year for holidays filter.</param>
        /// <returns></returns>
        Task<CommandResult> GetHolidaysAsync(string countryIsoCode, int year);
    }
}
