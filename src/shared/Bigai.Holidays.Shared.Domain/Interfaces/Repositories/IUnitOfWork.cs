using System;
using System.Threading.Tasks;

namespace Bigai.Holidays.Shared.Domain.Interfaces.Repositories
{
    /// <summary>
    /// <see cref="IUnitOfWork"/> represents a contract to save all changes to database.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Save all changes in this context to database.
        /// </summary>
        /// <returns><c>true</c> if the changes was written in database, otherwise <c>false</c>.</returns>
        Task<bool> CommitAsync();
    }
}
