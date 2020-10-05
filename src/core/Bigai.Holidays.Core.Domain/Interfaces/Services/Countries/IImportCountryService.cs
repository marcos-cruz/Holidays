using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Services;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.Countries
{
    /// <summary>
    /// <see cref="IImportCountryService"/> represents a contract to perform domain operations with CSV files for <see cref="Models.Country"/>.
    /// </summary>
    public interface IImportCountryService : IDomainService
    {
        /// <summary>
        /// This method imports country information from a csv file and returns a list of <see cref="Models.Country"/> lists.
        /// </summary>
        /// <param name="fileName">Name of csv file to import.</param>
        /// <returns><see cref="CommandResult"/> containing a list of <see cref="Models.Country"/> lists in the data property.</returns>
        CommandResult Import(string fileName);

        /// <summary>
        /// This method imports country information from a csv file and returns a list of <see cref="Models.Country"/> lists.
        /// </summary>
        /// <param name="fileName">Name of csv file to import.</param>
        /// <returns><see cref="CommandResult"/> containing a list of <see cref="Models.Country"/> lists in the data property.</returns>
        Task<CommandResult> ImportAsync(string fileName);
    }
}
