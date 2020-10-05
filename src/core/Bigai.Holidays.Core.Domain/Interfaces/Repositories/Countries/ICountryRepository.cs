using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;

namespace Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries
{
    /// <summary>
    /// <see cref="ICountryRepository"/> represents a contract to persist <see cref="Country"/> in relational database.
    /// </summary>
    public interface ICountryRepository : IRepository<Country>
    {
    }
}
