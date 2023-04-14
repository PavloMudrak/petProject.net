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

        public async Task DeleteAsync(string name)
        {
            await _customerProvider.DeleteCustomerAsync(name);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerProvider.GetAllCustomersAsync();
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, string sortColumn,
        string sortDirection, int pageIndex, int pageSize)
        {
            return await _customerProvider.SearchCustomersAsync(searchTerm, sortColumn, sortDirection, pageIndex, pageSize);
        }

        public async Task<int> CalculatePagesCount(int pageSize)
        {
            var recordsCount = await _customerProvider.GetRecordsCount();
            if ((recordsCount % pageSize) == 0)
            {
                return recordsCount / pageSize;
            }
            else 
            {
                return recordsCount / pageSize + 1; 
            }
        }
    }
}
