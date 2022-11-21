using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rabobank.TechnicalTest.GCOB.Dtos;
using Rabobank.TechnicalTest.GCOB.Services;

namespace Rabobank.TechnicalTest.GCOB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _customerService.GetCustomers();
                if (!result.Any())
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer customer)
        {
            try
            {
                var result = await _customerService.InsertCustomer(customer);

                if (result != null && result.Id > 0)
                    return Ok(result);

                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                return BadRequest();
            }
        }
    }
}
