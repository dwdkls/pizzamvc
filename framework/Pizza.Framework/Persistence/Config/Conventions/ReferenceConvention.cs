using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            var foreignKeyName = string.Format("FK__{0}_{1}__{2}", instance.EntityType.Name, instance.Name, instance.Class.Name);

            instance.ForeignKey(foreignKeyName);
            instance.Column(instance.Name + "Id");
            instance.Not.Nullable();
        }
    }
}