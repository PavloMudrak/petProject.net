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
        Task DeleteAsync(int id);
        Task<List<Customer>> GetAllCustomersAsync();
    }
}
