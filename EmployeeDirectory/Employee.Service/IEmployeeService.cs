namespace Employee.Service
{
    public interface IEmployeeService
    {
        DomainObject.Employee GetById(int id);
        void Create(DomainObject.Employee employee);
        void Update(DomainObject.Employee employee);
        void Delete(DomainObject.Employee employee);
    }
}
