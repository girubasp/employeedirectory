using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Employee.DomainObject.Mappings
{
    public class EmployeeMapping : IAutoMappingOverride<Employee>
    {
        public void Override(AutoMapping<Employee> mapping)
        {
            mapping.Id(map => map.Id).GeneratedBy.Identity();
            mapping.HasMany(x => x.PhoneNumbers);
        }
    }
}
