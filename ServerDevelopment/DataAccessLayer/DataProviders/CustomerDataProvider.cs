using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.DataProviders
{
    public class CustomerDataProvider : ICustomerDataProvider
    {
        private readonly AppDbContext _context;

        public CustomerDataProvider(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(string name)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Name == name);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm, 
            string sortColumn, string sortDirection, int pageIndex, int pageSize)
        {
            return await _context.Customers.FromSqlRaw("EXECUTE GetCustomersWithSortingAndPaging @p0, @p1, @p2, @p3, @p4",
                searchTerm, sortColumn, sortDirection, pageIndex, pageSize).ToListAsync();
        }
    }
}
