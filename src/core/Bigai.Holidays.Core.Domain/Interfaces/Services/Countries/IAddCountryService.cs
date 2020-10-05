using Bigai.Holidays.Core.Domain.Models.Countries;
using Bigai.Holidays.Shared.Domain.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.Countries
{
    /// <summary>
    /// <see cref="IAddCountryService"/> represents a contract to validate business rules and add <see cref="Country"/> to the database.
    /// </summary>
    public interface IAddCountryService
    {
        /// <summary>
        /// Add a <see cref="Country"/> in database.
        /// </summary>
        /// <param name="country">Instance of <see cref="Country"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="Country"/> added to the database.</returns>
        CommandResult Add(Country country);

        /// <summary>
        /// Add a <see cref="Country"/> in database.
        /// </summary>
        /// <param name="country">Instance of <see cref="Country"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="Country"/> added to the database.</returns>
        Task<CommandResult> AddAsync(Country country);

        /// <summary>
        /// Add a list of <see cref="Country"/> in database.
        /// </summary>
        /// <param name="listOfCountries">List of <see cref="Country"/> to be added.</param>
        CommandResult AddRange(List<Country> listOfCountries);

        /// <summary>
        /// Add a list of <see cref="Country"/> in database.
        /// </summary>
        /// <param name="listOfCountries">List of <see cref="Country"/> to be added.</param>
        Task<CommandResult> AddRangeAsync(List<Country> listOfCountries);

        /// <summary>
        /// Add a list of <see cref="Country"/> lists in database.
        /// </summary>
        /// <param name="listOfListCountries">List of <see cref="Country"/> lists to be added.</param>
        CommandResult AddRange(List<List<Country>> listOfListCountries);

        /// <summary>
        /// Add a list of <see cref="Country"/> lists in database.
        /// </summary>
        /// <param name="listOfListCountries">List of <see cref="Country"/> lists to be added.</param>
        Task<CommandResult> AddRangeAsync(List<List<Country>> listOfListCountries);
    }
}
