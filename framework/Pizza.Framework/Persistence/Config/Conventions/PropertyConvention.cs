using System;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Pizza.Framework.Utils;
using Pizza.Persistence.Attributes;

namespace Pizza.Framework.Persistence.Config.Conventions
{
    public class PropertyConvention : IPropertyConvention
    {
        public void Apply(IPropertyInstance instance)
        {
            // Default behaviour: any column can be null

            if (instance.Property.MemberInfo.GetAttribute<AllowNullAttribute>() != null)
            {
                instance.Nullable();
            }
            else
            {
                instance.Not.Nullable();
            }

            if (instance.Type == typeof(DateTime))
            {
                instance.CustomSqlType("DateTime2");
            }
            
            if (instance.Type == typeof(DateTime?))
            {
                instance.Nullable();
                instance.CustomSqlType("DateTime2");
            }
        }
    }
}