using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace Employee.DomainObject.Mappings
{
    public class PhoneNumberMapping : IAutoMappingOverride<PhoneNumber>
    {
        public void Override(AutoMapping<PhoneNumber> mapping)
        {
            mapping.Id(map => map.Id).GeneratedBy.Identity();
        }
    }
}
