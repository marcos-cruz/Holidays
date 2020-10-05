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
        /// Add a <see cref="TEntity"/> in database.
        /// </summary>
        /// <param name="entity">Instance of <see cref="TEntity"/> to be added.</param>
        /// <returns><see cref="TEntity"/> that was added to the database.</returns>
        TEntity Add(TEntity entity);

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
        void AddRange(List<TEntity> entities);

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
        TEntity Update(TEntity entity);

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
        /// Remove a <see cref="TEntity"/> in database.
        /// </summary>
        /// <param name="id">Id of <see cref="TEntity"/> to be removed.</param>
        void Remove(Guid id);

        /// <summary>
        /// Gets an <see cref="TEntity"/> by its id.
        /// </summary>
        /// <param name="id">Id that identifies the entity in the database.</param>
        /// <returns>Instance of the <see cref="TEntity"/> corresponding to the Id, <c>null</c> if it does not exist.</returns>
        Task<TEntity> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets an <see cref="TEntity"/> by its id.
        /// </summary>
        /// <param name="id">Id that identifies the entity in the database.</param>
        /// <returns>Instance of the <see cref="TEntity"/> corresponding to the Id, <c>null</c> if it does not exist.</returns>
        TEntity GetById(Guid id);

        /// <summary>
        /// Gets an lists of <see cref="TEntity"/> according to an expression.
        /// </summary>
        /// <param name="predicate">Expression to be tested.</param>
        /// <returns>Lists of <see cref="TEntity"/> according to an expression.</returns>
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Gets an lists of <see cref="TEntity"/> according to an expression.
        /// </summary>
        /// <param name="predicate">Expression to be tested.</param>
        /// <returns>Lists of <see cref="TEntity"/> according to an expression.</returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Save all changes in this context to database.
        /// </summary>
        /// <returns>Number os entities written in database.</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Save all changes in this context to database.
        /// </summary>
        /// <returns>Number os entities written in database.</returns>
        int SaveChanges();
    }
}
