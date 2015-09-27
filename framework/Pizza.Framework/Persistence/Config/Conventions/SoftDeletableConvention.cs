using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Contracts.Persistence;
using Pizza.Framework.Persistence.SoftDelete;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class SoftDeletableConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(instance.EntityType))
            {
                instance.ApplyFilter<SoftDeletableFilter>();
            }
        }
    }
}