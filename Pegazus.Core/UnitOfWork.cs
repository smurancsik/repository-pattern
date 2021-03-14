using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Pegazus.Core.Interfaces;

namespace Pegazus.Core
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private bool _disposed;
        private Dictionary<Type, object> _repositories;
        private TransactionScope _scope;

        /// <summary>
        /// Initializes a new instance of the UnitOfWork<TContext>.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="serviceProvider"></param>
        public UnitOfWork(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public TContext DbContext => _context;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            Type type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new RepositoryBase<TEntity>(_context);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public void BeginTransaction()
        {
            BeginTransaction(new TransactionOptions
            {
                IsolationLevel = IsolationLevel.RepeatableRead
            });
        }

        public void BeginTransaction(TransactionOptions options)
        {
            _scope?.Dispose();

            _scope = new TransactionScope(TransactionScopeOption.Required, options,
                TransactionScopeAsyncFlowOption.Enabled);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void CommitTransaction()
        {
            _scope?.Complete();
            _scope?.Dispose();
        }

        public void RollbackTransaction()
        {
            if (_scope == null)
            {
                return;
            }

            _scope.Dispose();
            _scope = null;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _scope?.Dispose();
                    _repositories?.Clear();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
