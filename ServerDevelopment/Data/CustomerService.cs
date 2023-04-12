using DataAccessLayer.DataProviders;
using DataAccessLayer.Models;

namespace ServerDevelopment.Data
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDataProvider _customerProvider;

        public CustomerService(ICustomerDataProvider customerService)
        {
            _customerProvider = customerService;
        }

        public async Task CreateAsync(Customer customer)
        {
            await _customerProvider.AddCustomerAsync(customer);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _customerProvider.GetCustomerByIdAsync(id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _customerProvider.UpdateCustomerAsync(customer);
        }

        public async Task DeleteAsync(int id)
        {
            await _customerProvider.DeleteCustomerAsync(id);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerProvider.GetAllCustomersAsync();
        }
    }
}
