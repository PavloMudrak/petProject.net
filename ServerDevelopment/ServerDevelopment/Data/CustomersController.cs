using DataAccessLayer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServerDevelopment.Data.other;

namespace ServerDevelopment.Data
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
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
                var result = await _customerService.UpdateAsync(name, customer);
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

        [HttpGet]
        public async Task<ActionResult<SearchCustomersResponse>> SearchCustomers([FromQuery] SearchCustomersRequest request)
        {
            try
            {
                var customers = await _customerService.SearchCustomersAsync(request.Query, request.SortColumn.ToString(), request.SortOrder.ToString(), request.PageIndex, request.PageSize);
                var response = new SearchCustomersResponse
                {
                    Customers = (List<CustomerDTO>)customers.Customers,
                    PagesCount = _customerService.CalculatePagesCount(request.PageSize, customers.TotalRows)
                };
                return Ok(response);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
