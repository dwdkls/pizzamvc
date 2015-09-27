using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            //            instance.GeneratedBy.Increment();
            instance.GeneratedBy.HiLo("1000");
        }
    }
}