using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Pegazus.Core.Interfaces
{
    // Source: https://blog.dcube.fr/index.php/2019/09/05/generic-repository-unit-of-work-et-entity-framework/
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        /// <summary>
        /// Gets the db context.
        /// </summary>
        /// <returns>The instance of type TContext.</returns>
        TContext DbContext { get; }

        /// <summary>
        /// Gets the specified repository for the TEntity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>An instance of type inherited from GenericRepository interface.</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : EntityBase;

        /// <summary>
        /// Begins transaction with default isolation level.
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Begins transaction with provided isolation level.
        /// </summary>
        /// <param name="option"></param>
        void BeginTransaction(TransactionOptions option);

        /// <summary>
        /// Commit all changes made in this context to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        int Save();

        /// <summary>
        /// Commits transaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rolls back the transaction, if transaction was started.
        /// </summary>
        void RollbackTransaction();
    }
}
