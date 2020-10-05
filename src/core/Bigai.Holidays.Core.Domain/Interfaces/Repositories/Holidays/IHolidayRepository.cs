using Bigai.Holidays.Core.Domain.Models.Holidays;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;

namespace Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays
{
    /// <summary>
    /// <see cref="IHolidayRepository"/> represents a contract to persist <see cref="Holiday"/> in relational database.
    /// </summary>
    public interface IHolidayRepository : IRepository<Holiday>
    {
    }
}
