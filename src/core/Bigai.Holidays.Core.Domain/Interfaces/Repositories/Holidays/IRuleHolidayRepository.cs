using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;

namespace Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays
{
    /// <summary>
    /// <see cref="IRuleHolidayRepository"/> represents a contract to persist <see cref="RuleHoliday"/> in relational database.
    /// </summary>
    public interface IRuleHolidayRepository : IRepository<RuleHoliday>
    {
    }
}
