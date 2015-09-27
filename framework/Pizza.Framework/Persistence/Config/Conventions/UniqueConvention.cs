using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Contracts.Persistence.Attributes;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class UniqueConvention : AttributePropertyConvention<UniqueAttribute>
    {
        protected override void Apply(UniqueAttribute attribute, IPropertyInstance instance)
        {
            instance.Unique();
            //            instance.UniqueKey(string.Format("Ulala__{0}_{1}", instance.EntityType.Name, instance.Name));
        }
    }
}