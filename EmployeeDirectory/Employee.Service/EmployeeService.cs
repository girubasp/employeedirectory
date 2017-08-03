using DataAccess.Core;

namespace Employee.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository _repository;

        public EmployeeService(IRepository repository)
        {
            _repository = repository;
        }

        public DomainObject.Employee GetById(int id)
        {
            return _repository.GetById<DomainObject.Employee, int>(id);
        }

        public void Create(DomainObject.Employee employee)
        {
            _repository.Insert(employee);
        }

        public void Update(DomainObject.Employee employee)
        {
            _repository.Update(employee);
        }

        public void Delete(DomainObject.Employee employee)
        {
            _repository.Delete(employee);
        }
    }
}
