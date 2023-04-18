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

        public async Task<(IEnumerable<Customer> Customers, int TotalRows)> SearchCustomersAsync2(string searchTerm, string sortColumn, string sortDirection, int pageIndex, int pageSize)
        {
            var result = await _customerProvider.SearchCustomersAsync2(searchTerm, sortColumn, sortDirection, pageIndex, pageSize);
            return result;
        }


        public async Task<int> CalculatePagesCount(int pageSize, int totalRows)
        {
            if(totalRows == 0)
            {
                var a = _customerProvider.FillDbByRandomData();
            }
            if ((totalRows % pageSize) == 0)
            {
                return totalRows / pageSize;
            }
            else
            {
                return totalRows / pageSize + 1;
            }
        }
    }
}
