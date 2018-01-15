using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            var foreignKeyName = $"FK__{instance.EntityType.Name}_{instance.Name}__{instance.Class.Name}";

            instance.ForeignKey(foreignKeyName);
            instance.Column(instance.Name + "Id");
            instance.Not.Nullable();
        }
    }
}