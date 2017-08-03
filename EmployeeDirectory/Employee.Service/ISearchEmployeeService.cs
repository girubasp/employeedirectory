using DataAccess.Core;
using Employee.Common;

namespace Employee.Service
{
    public interface ISearchEmployeeService
    {
        PagedResult<DomainObject.Employee> SearchEmployees(SearchBy searchBy, string searchTerm, int page, int pageSize);
    }
}
