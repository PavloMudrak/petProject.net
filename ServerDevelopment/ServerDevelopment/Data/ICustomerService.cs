﻿using DataAccessLayer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerDevelopment.Data
{
    public interface ICustomerService
    {
        Task CreateAsync(Customer customer);
        Task<CustomerDTO> GetByIdName(string name);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(string name);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<int> CalculatePagesCount(int pageSize, int totalRows);
        Task<(IEnumerable<CustomerDTO> Customers, int TotalRows)> SearchCustomersAsync(
            string searchTerm, string sortColumn, string sortDirection, int pageIndex, int pageSize);
    }
}
