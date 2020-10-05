using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Countries;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.Holidays;
using Bigai.Holidays.Core.Domain.Interfaces.Repositories.States;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;

namespace Bigai.Holidays.Core.Domain.Interfaces.Repositories
{
    /// <summary>
    /// <see cref="IUnitOfWorkCore"/> represents a contract to save all changes to database.
    /// </summary>
    public interface IUnitOfWorkCore : IUnitOfWork
    {
        /// <summary>
        /// Performs read and write operations in the country repository.
        /// </summary>
        public ICountryRepository CountryRepository { get; }

        /// <summary>
        /// Performs read and write operations in the state repository.
        /// </summary>
        public IStateRepository StateRepository { get; }

        /// <summary>
        /// Performs read and write operations in the rule holiday repository.
        /// </summary>
        public IRuleHolidayRepository RuleHolidayRepository { get; }

        /// <summary>
        /// Performs read and write operations in the holiday repository.
        /// </summary>
        public IHolidayRepository HolidayRepository { get; }
    }
}
