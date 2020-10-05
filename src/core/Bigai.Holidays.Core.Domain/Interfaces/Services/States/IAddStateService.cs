using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Commands;
using Bigai.Holidays.Shared.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Interfaces.Services.States
{
    /// <summary>
    /// <see cref="IAddStateService"/> represents a contract to validate business rules and add <see cref="State"/> to the database.
    /// </summary>
    public interface IAddStateService : IDomainService
    {
        /// <summary>
        /// Add a <see cref="State"/> in database.
        /// </summary>
        /// <param name="state">Instance of <see cref="State"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="State"/> added to the database.</returns>
        CommandResult Add(State state);

        /// <summary>
        /// Add a <see cref="State"/> in database.
        /// </summary>
        /// <param name="state">Instance of <see cref="State"/> to be added.</param>
        /// <returns>Returns an instance of <see cref="CommandResult"/> containing the operation status code  and the <see cref="State"/> added to the database.</returns>
        Task<CommandResult> AddAsync(State state);

        /// <summary>
        /// Add a list of <see cref="State"/> in database.
        /// </summary>
        /// <param name="listOfStates">List of <see cref="State"/> to be added.</param>
        CommandResult AddRange(List<State> listOfStates);

        /// <summary>
        /// Add a list of <see cref="State"/> in database.
        /// </summary>
        /// <param name="listOfStates">List of <see cref="State"/> to be added.</param>
        Task<CommandResult> AddRangeAsync(List<State> listOfStates);

        /// <summary>
        /// Add a list of <see cref="State"/> lists in database.
        /// </summary>
        /// <param name="listOfListStates">List of <see cref="State"/> lists to be added.</param>
        CommandResult AddRange(List<List<State>> listOfListStates);

        /// <summary>
        /// Add a list of <see cref="State"/> lists in database.
        /// </summary>
        /// <param name="listOfListStates">List of <see cref="State"/> lists to be added.</param>
        Task<CommandResult> AddRangeAsync(List<List<State>> listOfListStates);
    }
}
