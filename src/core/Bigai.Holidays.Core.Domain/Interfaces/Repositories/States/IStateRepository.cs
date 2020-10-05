using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;

namespace Bigai.Holidays.Core.Domain.Interfaces.Repositories.States
{
    /// <summary>
    /// <see cref="IStateRepository"/> represents a contract to persist <see cref="State"/> in relational database.
    /// </summary>
    public interface IStateRepository : IRepository<State>
    {
    }
}
