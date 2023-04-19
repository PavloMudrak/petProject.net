using DataAccessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ServerDevelopment.Data
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var customer = await _customerService.GetByName(name);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDTO customer)
        {
            try
            {
                var result = await _customerService.CreateAsync(customer);
                if (!result.IsValid)
                {
                    return Conflict(result.Errors);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, CustomerDTO customer)
        {
            try
            {
                var result = await _customerService.UpdateAsync(name ,customer);
                if (!result.IsValid)
                {
                    return Conflict(result.Errors);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            try
            {
                await _customerService.DeleteAsync(name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomers(
            [FromQuery] int pageSize = 10,
            [FromQuery] int pageIndex = 1,
            [FromQuery] string? searchTerm = "",
            [FromQuery] string? sortColumn = "Name",
            [FromQuery] string? sortOrder = "ASC")
        {
            try
            {
                var customers = await _customerService.SearchCustomersAsync(searchTerm, sortColumn, sortOrder, pageIndex, pageSize);
                var result = new
                {
                    Customers = customers.Customers,
                    PagesCount = await _customerService.CalculatePagesCount(pageSize, customers.TotalRows)
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
