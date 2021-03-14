
using System.Collections.Generic;
using Pegazus.Logic.Models;

namespace Pegazus.Logic.Interfaces
{
    public interface ICustomerLogic
    {
        IList<CustomerModel> GetAll();
    }
}
