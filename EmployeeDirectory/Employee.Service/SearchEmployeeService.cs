using DataAccess.Core;
using Employee.Common;
using Employee.DomainObject.Query;

namespace Employee.Service
{
    public class SearchEmployeeService : ISearchEmployeeService
    {
        private readonly IRepository _repository;

        public SearchEmployeeService(IRepository repository)
        {
            _repository = repository;
        }
        public PagedResult<DomainObject.Employee> SearchEmployees(SearchBy searchBy, string searchTerm, int page, int pageSize = 10)
        {
            return _repository.Find(new SearchEmployeeQuery(searchBy, searchTerm, page, pageSize));
        }
    }
}
