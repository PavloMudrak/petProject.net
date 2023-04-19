using AutoMapper;
using DataAccessLayer.DataProviders;
using DataAccessLayer.Models;

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

        public async Task CreateAsync(Customer customer)
        {
            await _customerProvider.AddCustomerAsync(customer);
        }

        public async Task<CustomerDTO> GetByName(string name)
        {
            var result = await _customerProvider.GetCustomerByNameAsync(name);
            var customerDTO = _mapper.Map<CustomerDTO>(result);
            return customerDTO;
        }

        public async Task<FluentValidation.Results.ValidationResult> UpdateAsync(string oldName, CustomerDTO customer)
        {
            var validator = new CustomerValidator(this, oldName);
            var validatorResult = await validator.ValidateAsync(customer);

            if (validatorResult.IsValid)
            {
                var customerFromDb = await _customerProvider.GetCustomerByNameAsync(oldName);
                var updatedCustomer = _mapper.Map<Customer>(customer);

                customerFromDb.Name = updatedCustomer.Name;
                customerFromDb.Phone = updatedCustomer.Phone;
                customerFromDb.CompanyName = updatedCustomer.CompanyName;
                customerFromDb.EmailAddress = updatedCustomer.EmailAddress;

                await _customerProvider.UpdateCustomerAsync(customerFromDb);
            }
            return validatorResult;

        }

        public async Task DeleteAsync(string name)
        {
            await _customerProvider.DeleteCustomerAsync(name);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerProvider.GetAllCustomersAsync();
        }

        public async Task<(IEnumerable<CustomerDTO> Customers, int TotalRows)> SearchCustomersAsync(string searchTerm, string sortColumn, string sortDirection, int pageIndex, int pageSize)
        {
            var result = await _customerProvider.SearchCustomersAsync2(searchTerm, sortColumn, sortDirection, pageIndex, pageSize);
            var customersDTO = _mapper.Map<List<CustomerDTO>>(result.Customers);
            return (customersDTO, result.TotalRows);
        }


        public async Task<int> CalculatePagesCount(int pageSize, int totalRows)
        {
            if (totalRows == 0)
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

        public async Task<bool> IsNameUniqueAsync(string newName, string oldName)
        {
            if (oldName == newName && oldName != "")
            {
                return true;
            }
            else
            {
                var customer = await _customerProvider.GetCustomerByNameAsync(newName);
                if (customer == null)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
