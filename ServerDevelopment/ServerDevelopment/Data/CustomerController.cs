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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Customer customer)
        {
            try
            {
                await _customerService.CreateAsync(customer);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Customer customer)
        {
            try
            {
                if (id != customer.Id)
                {
                    return BadRequest();
                }
                await _customerService.UpdateAsync(customer);
                return Ok();
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
