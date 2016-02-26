using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Persistence.Attributes;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class FixedLengthStringConvention : AttributePropertyConvention<FixedLengthUnicodeStringAttribute>
    {
        protected override void Apply(FixedLengthUnicodeStringAttribute attribute, IPropertyInstance instance)
        {
            instance.CustomSqlType(string.Format("nchar({0})", attribute.Length));
        }
    }
}