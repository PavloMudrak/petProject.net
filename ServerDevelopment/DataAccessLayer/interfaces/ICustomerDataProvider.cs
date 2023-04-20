using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.interfaces
{
    public interface ICustomerDataProvider
    {
        Task<Customer> GetCustomerByNameAsync(string name);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string name);
        Task<int> FillDbByRandomData();
        Task<(IEnumerable<Customer> Customers, int TotalRows)> SearchCustomersAsync(
            string searchTerm, string sortColumn, string sortDirection, int pageIndex, int pageSize);
    }
}
