using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var randomCustomers = GenerateRandomCustomers(1000);
            foreach(var customer in randomCustomers)
            {
                await _context.Customers.AddAsync(customer);
            }
            await _context.SaveChangesAsync();
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


        ///////////////////////////////////
        ///

        public List<Customer> GenerateRandomCustomers(int count)
        {
            var customers = new List<Customer>();

            var random = new Random();

            for (int i = 0; i < count; i++)
            {
                var name = GetRandomName(random);
                var companyName = GetRandomCompanyName(random);
                var phone = GetRandomPhone(random);
                var email = GetRandomEmail(random);

                var customer = new Customer
                {
                    Name = name + i,
                    CompanyName = companyName,
                    Phone = phone,
                    Email = email
                };

                customers.Add(customer);
            }

            return customers;
        }

        private string GetRandomName(Random random)
        {
            var names = new List<string> { "John", "Mary", "Robert", "Emily", "William", "Emma", "David", "Linda", "Michael", "Susan" };
            return names[random.Next(names.Count)];
        }

        private string GetRandomCompanyName(Random random)
        {
            var suffixes = new List<string> { "Inc", "LLC", "Corp", "Ltd" };
            return $"{GetRandomName(random)} {suffixes[random.Next(suffixes.Count)]}";
        }

        private string GetRandomPhone(Random random)
        {
            var builder = new StringBuilder();
            builder.Append("(");
            builder.Append(random.Next(100, 1000).ToString());
            builder.Append(") ");
            builder.Append(random.Next(100, 1000).ToString());
            builder.Append("-");
            builder.Append(random.Next(1000, 10000).ToString());
            return builder.ToString();
        }

        private string GetRandomEmail(Random random)
        {
            var domains = new List<string> { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "aol.com" };
            return $"{GetRandomName(random).ToLower()}.{GetRandomName(random).ToLower()}@{domains[random.Next(domains.Count)]}";
        }
    }
}
