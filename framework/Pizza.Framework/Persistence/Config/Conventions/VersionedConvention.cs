using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Persistence;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class VersionedConvention : IVersionConvention
    {
        public void Apply(IVersionInstance instance)
        {
            if (typeof(IVersionable).IsAssignableFrom(instance.EntityType))
            {
                instance.Generated.Always();
            }
        }
    }
}