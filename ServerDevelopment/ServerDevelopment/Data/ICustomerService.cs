using DataAccessLayer.Models;
using ServerDevelopment.Data.other;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerDevelopment.Data
{
    public interface ICustomerService
    {
        Task CreateAsync(CustomerDTO customer);
        Task<CustomerDTO> GetByNameAsync(string name);
        Task UpdateAsync(string oldName, CustomerDTO customer);
        Task DeleteAsync(string name);
        Task<SearchCustomersResponse> SearchCustomersAsync(
            string searchTerm, string sortColumn, string sortDirection, int pageIndex, int pageSize);
    }
}
