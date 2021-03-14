using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pegazus.Core.Interfaces;

namespace Pegazus.Core
{
    // Source: https://blog.dcube.fr/index.php/2019/09/05/generic-repository-unit-of-work-et-entity-framework/
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        public DbContext DbContext { get; }
        protected readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the RepositoryBase<TEntity>.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public RepositoryBase(DbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = DbContext.Set<TEntity>();
        }

        #region CREATE

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Add(IEnumerable<TEntity> entities) => _dbSet.AddRange(entities);

        #endregion

        #region READ

        public virtual TEntity GetById(params object[] keyValues) => _dbSet.Find(keyValues);

        public virtual TEntity GetFirstOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
        )
        {
            IQueryable<TEntity> query = ParameterUpQuery(_dbSet, predicate, include, disableTracking);

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }

        public virtual TEntity GetSingleOrDefault(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
        )
        {
            IQueryable<TEntity> query = ParameterUpQuery(_dbSet, predicate, include, disableTracking);

            if (orderBy != null)
            {
                return orderBy(query).SingleOrDefault();
            }
            else
            {
                return query.SingleOrDefault();
            }
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public virtual IEnumerable<TEntity> GetMultiple(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true
        )
        {
            IQueryable<TEntity> query = ParameterUpQuery(_dbSet, predicate, include, disableTracking);

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        private IQueryable<TEntity> ParameterUpQuery(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true)
        {
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sql, parameters);
        }

        #endregion

        #region UPDATE

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

        #endregion

        #region DELETE

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);

            if (entityToDelete != null)
            {
                _dbSet.Remove(entityToDelete);
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Delete(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

        #endregion

        #region OTHER

        public virtual int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }

        public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        #endregion
    }
}
