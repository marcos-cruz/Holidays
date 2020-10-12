using Bigai.Holidays.Core.Infra.Data.Contexts;
using Bigai.Holidays.Shared.Domain.Interfaces.Repositories;
using Bigai.Holidays.Shared.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Infra.Data.Repositories.Abstracts
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        #region Protected Variables

        protected HolidaysContext DbContext;

        private bool _databaseExist = false;
        private bool _disposed = false;

        #endregion

        #region Constructor

        protected Repository(HolidaysContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        #endregion

        #region Public Methods

        public virtual async Task<bool> DatabaseExistAsync()
        {
            try
            {
                if (!_databaseExist)
                {
                    _databaseExist = await DbContext.Database.GetService<IRelationalDatabaseCreator>().ExistsAsync();
                }
            }
            catch (Exception ex) { throw ex; }

            return _databaseExist;
        }

        public virtual async Task<bool> CreateDatabaseAsync()
        {
            bool databaseCreated = false;
            try
            {
                if (!_databaseExist)
                {
                    databaseCreated = await DbContext.Database.EnsureCreatedAsync();
                    _databaseExist = true;
                }

            }
            catch (Exception ex) { throw ex; }

            return databaseCreated;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            try
            {
                await DbContext.Set<TEntity>().AddAsync(entity);

                return entity;
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual async Task AddRangeAsync(List<TEntity> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return;
            }

            try
            {
                await DbContext.Set<TEntity>().AddRangeAsync(entities);
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return (await Task.Run(() => Update(entity)));
        }

        public virtual async Task RemoveAsync(Guid id)
        {
            try
            {
                TEntity obj = await DbContext.Set<TEntity>().FindAsync(id);

                DbContext.Set<TEntity>().Remove(obj);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return (await DbContext.Set<TEntity>().Where(predicate).ToListAsync()).Count();
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await DbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                return await DbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex) { throw ex; }
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            try
            {
                return await DbContext.SaveChangesAsync();
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region Private Methods

        private TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            try
            {
                DbContext.Set<TEntity>().Update(entity);
            }
            catch (Exception ex) { throw ex; }

            return entity;
        }

        #endregion

        #region Public Dispose

        public void Dispose()
        {
            if (DbContext != null)
            {
                try
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
                catch (Exception ex) { throw ex; }
            }
        }

        private void Dispose(bool disposing)
        {
            try
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        DbContext.Dispose();
                    }
                }
            }
            catch (Exception ex) { throw ex; }

            _disposed = true;
        }

        #endregion
    }
}
