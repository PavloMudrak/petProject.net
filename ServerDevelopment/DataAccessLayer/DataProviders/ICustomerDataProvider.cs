using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataProviders
{
    public interface ICustomerDataProvider
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string name);
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm,
            string sortColumn, string sortDirection, int pageIndex, int pageSize);
        Task<int> GetRecordsCount();
    }
}
