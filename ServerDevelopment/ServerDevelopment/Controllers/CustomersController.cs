using DataAccessLayer.Models;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ServerDevelopment.Data;
using ServerDevelopment.Data.other;
using ServerDevelopment.Interfaces;

namespace ServerDevelopment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IValidator<CustomerDTO> _customerValidator;
        private readonly ICustomerService _customerService;

        public CustomersController(IValidator<CustomerDTO> customerValidator, ICustomerService customerService)
        {
            _customerValidator = customerValidator;
            _customerService = customerService;
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var customer = await _customerService.GetByNameAsync(name);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerDTO customer)
        {
            var validationResult = await _customerValidator.ValidateAsync(customer);
            if (!validationResult.IsValid)
                return Conflict(validationResult.Errors);

            await _customerService.CreateAsync(customer);
            return NoContent();
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> Put(string name, CustomerDTO customer)
        {
            var validationResult = await _customerValidator.ValidateAsync(customer);
            if (!validationResult.IsValid)
                return Conflict(validationResult.Errors);

            await _customerService.UpdateAsync(name, customer);
            return NoContent();
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            await _customerService.DeleteAsync(name);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<SearchCustomersResponse>> SearchCustomers([FromQuery] SearchCustomersRequest request)
        {
            var response = await _customerService.SearchCustomersAsync(
                request.Query, request.SortColumn.ToString(), request.SortOrder.ToString(), request.PageIndex, request.PageSize);
            return Ok(response);
        }
    }
}
