using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerDevelopment.Data
{
    public interface ICustomerService
    {
        Task CreateAsync(Customer customer);
        Task<Customer> GetByIdAsync(int id);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(string name);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, string sortColumn,
        string sortDirection, int pageIndex, int pageSize);
        Task<int> CalculatePagesCount(int pageSize);
    }
}
