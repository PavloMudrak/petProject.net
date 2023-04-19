using AutoMapper;
using DataAccessLayer.DataProviders;
using DataAccessLayer.Models;
using FluentValidation;

namespace ServerDevelopment.Data
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDataProvider _customerProvider;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerDataProvider customerService, IMapper customersMapper)
        {
            _customerProvider = customerService;
            _mapper = customersMapper;
        }

        public async Task CreateAsync(CustomerDTO customer)
        {
            var customerForDb = _mapper.Map<Customer>(customer);
            await _customerProvider.AddCustomerAsync(customerForDb);
        }

        public async Task<CustomerDTO> GetByNameAsync(string name)
        {
            var result = await _customerProvider.GetCustomerByNameAsync(name);
            var customerDTO = _mapper.Map<CustomerDTO>(result);
            return customerDTO;
        }

        public async Task UpdateAsync(string oldName,CustomerDTO customer)
        {
            var customerFromDb = await _customerProvider.GetCustomerByNameAsync(oldName);
            var updatedCustomer = _mapper.Map<Customer>(customer);

            customerFromDb.Name = updatedCustomer.Name;
            customerFromDb.Phone = updatedCustomer.Phone;
            customerFromDb.CompanyName = updatedCustomer.CompanyName;
            customerFromDb.EmailAddress = updatedCustomer.EmailAddress;

            await _customerProvider.UpdateCustomerAsync(customerFromDb);

        }

        public async Task DeleteAsync(string name)
        {
            await _customerProvider.DeleteCustomerAsync(name);
        }

        public async Task<(IEnumerable<CustomerDTO> Customers, int TotalRows)> SearchCustomersAsync(string searchTerm, string sortColumn, string sortDirection, int pageIndex, int pageSize)
        {
            var result = await _customerProvider.SearchCustomersAsync(searchTerm, sortColumn, sortDirection, pageIndex, pageSize);
            var customersDTO = _mapper.Map<List<CustomerDTO>>(result.Customers);
            return (customersDTO, result.TotalRows);
        }


        public int CalculatePagesCount(int pageSize, int totalRows)
        {
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
