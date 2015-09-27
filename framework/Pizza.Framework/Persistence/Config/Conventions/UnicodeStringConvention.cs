using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Contracts.Persistence.Attributes;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class UnicodeStringConvention : AttributePropertyConvention<UnicodeStringAttribute>
    {
        protected override void Apply(UnicodeStringAttribute attribute, IPropertyInstance instance)
        {
            instance.Length(attribute.Length);
        }
    }
}