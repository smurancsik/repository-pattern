using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Pegazus.API.Dto;
using Pegazus.Core.Extensions;
using Pegazus.Logic.Interfaces;
using Pegazus.Logic.Models;

namespace Pegazus.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        #region Properties

        /// <summary>
        /// Reference the customer business logic.
        /// </summary>
        private readonly ICustomerLogic _customerLogic;

        /// <summary>
        /// Reference the auto mapper.
        /// </summary>
        private readonly IMapper _mapper;

        #endregion

        #region ctor

        public CustomerController(ICustomerLogic customerLogic, IMapper mapper)
        {
            _customerLogic = customerLogic ?? throw new Exception();
            _mapper = mapper ?? throw new Exception();
        }

        #endregion

        #region Additional methods

        // GET api/customer
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<CustomerDto>> Get()
        {
            try
            {
                IList<CustomerModel> result = _customerLogic.GetAll();

                if (result.Any())
                {
                    return Ok(_mapper.MapCollection<CustomerModel, CustomerDto>(result));
                }

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        #endregion
    }
}
