using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Persistence.Attributes;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class FixedLengthAnsiStringConvention : AttributePropertyConvention<FixedLengthAnsiStringAttribute>
    {
        protected override void Apply(FixedLengthAnsiStringAttribute attribute, IPropertyInstance instance)
        {
            instance.CustomSqlType($"char({attribute.Length})");
        }
    }
}