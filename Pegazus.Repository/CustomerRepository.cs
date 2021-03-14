using Pegazus.Core;
using Pegazus.Domain.Models;
using Pegazus.Repository.Interfaces;

namespace Pegazus.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        #region ctor

        /// <summary>
        /// Initializes a new instance of Customer Repository.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CustomerRepository(PegazusContext dbContext)
            : base(dbContext)
        {
        }

        #endregion
    }
}
