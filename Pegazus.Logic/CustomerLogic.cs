using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Pegazus.Core.Extensions;
using Pegazus.Domain.Models;
using Pegazus.Logic.Interfaces;
using Pegazus.Logic.Models;
using Pegazus.Repository.Interfaces;

namespace Pegazus.Logic
{
    public class CustomerLogic : ICustomerLogic
    {
        #region Properties

        /// <summary>
        /// Reference for the customer repository.
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// Reference for the auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region ctor

        public CustomerLogic(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new Exception();
            _mapper = mapper ?? throw new Exception();
        }

        #endregion

        #region ILogic methods
        
        public IList<CustomerModel> GetAll()
        {
            List<Customer> customers = _customerRepository.GetAll().ToList();

            if (customers.Any())
            {
                return _mapper.MapCollection<Customer, CustomerModel>(customers);
            }

            return null;
        }

        #endregion
    }
}
