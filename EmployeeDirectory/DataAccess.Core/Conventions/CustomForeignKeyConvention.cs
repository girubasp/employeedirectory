using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace DataAccess.Core.Conventions
{
    public class ForeignKeyNameConvention : IHasManyConvention
    {

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
            instance.Cascade.AllDeleteOrphan();
        }
    }
}
