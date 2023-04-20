using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;

namespace ServerDevelopment.Data.other
{
    public class SearchCustomersRequest
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;
        public string? Query { get; set; } = "";
        public SortColumn SortColumn { get; set; } = SortColumn.Name;
        public SortOrder SortOrder { get; set; } = SortOrder.ASC;
    }

    public class SearchCustomersResponse
    {
        public List<CustomerDTO> Customers { get; set; }
        public int PagesCount { get; set; }
    }


    public enum SortColumn
    {
        Name,
        Phone,
        EmailAddress,
        CompanyName
    }

    public enum SortOrder
    {
        ASC,
        DESC
    }
}
