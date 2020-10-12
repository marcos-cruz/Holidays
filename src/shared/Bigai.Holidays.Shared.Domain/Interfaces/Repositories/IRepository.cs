using Bigai.Holidays.Shared.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bigai.Holidays.Shared.Domain.Interfaces.Repositories
{
    /// <summary>
    /// <see cref="IRepository{TEntity}"/> represents a contract to persist objects in relational database.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        /// <summary>
        /// Determines whether the database already exists.
        /// </summary>
        /// <returns><c>true</c> if database exist, otherwise <c>false</c>.</returns>
        Task<bool> DatabaseExistAsync();

        /// <summary>
        /// Creates the database.
        /// </summary>
        Task<bool> CreateDatabaseAsync();

        /// <summary>
        /// Add a <see cref="TEntity"/> in database.
        /// </summary>
        /// <param name="entity">Instance of <see cref="TEntity"/> to be added.</param>
        /// <returns><see cref="TEntity"/> that was added to the database.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Add a list of <see cref="TEntity"/> in database.
        /// </summary>
        /// <param name="entities">List of instances of <see cref="TEntity"/> to be added.</param>
        /// <returns></returns>
        Task AddRangeAsync(List<TEntity> entities);

        /// <summary>
        /// Update a <see cref="TEntity"/> in database.
        /// </summary>
        /// <param name="entity">Instance of <see cref="TEntity"/> to be updated.</param>
        /// <returns><see cref="TEntity"/> that was updated to the database.</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Remove a <see cref="TEntity"/> in database.
        /// </summary>
        /// <param name="id">Id of <see cref="TEntity"/> to be removed.</param>
        Task RemoveAsync(Guid id);

        /// <summary>
        /// Determines the number of records that satisfy the query filter.
        /// </summary>
        /// <param name="predicate">Expression to be tested.</param>
        /// <returns>The number od records taht satisfy the query filter.</returns>
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an <see cref="TEntity"/> by its id.
        /// </summary>
        /// <param name="id">Id that identifies the entity in the database.</param>
        /// <returns>Instance of the <see cref="TEntity"/> corresponding to the Id, <c>null</c> if it does not exist.</returns>
        Task<TEntity> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets an lists of <see cref="TEntity"/> according to an expression.
        /// </summary>
        /// <param name="predicate">Expression to be tested.</param>
        /// <returns>Lists of <see cref="TEntity"/> according to an expression.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Save all changes in this context to database.
        /// </summary>
        /// <returns>Number os entities written in database.</returns>
        Task<int> SaveChangesAsync();
    }
}
