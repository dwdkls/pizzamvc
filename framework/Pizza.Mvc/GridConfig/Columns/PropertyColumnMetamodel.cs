using System;
using System.Linq.Expressions;
using Pizza.Utils;

namespace Pizza.Mvc.GridConfig.Columns
{
    public sealed class PropertyColumnMetamodel : ColumnMetamodelBase
    {
        public Type PropertyType { get; private set; }
        public FilterMetamodel Filter { get; private set; }

        public PropertyColumnMetamodel(string name, string caption, int width, ColumnWidthMode widthMode, Type propertyType, FilterMetamodel filter)
            : base(name, caption, width, widthMode)
        {
            this.PropertyType = propertyType;
            this.Filter = filter;
        }

        public static PropertyColumnMetamodel Create(LambdaExpression property, int width, ColumnWidthMode widthMode, FilterMetamodel filterMetamodel)
        {
            var name = ObjectHelper.GetPropertyName(property);
            var caption = AttributesHelper.GetPropertyDisplayName(property) ?? property.Name;   // TODO: maybe better to provide explicite caption parameter?

            return new PropertyColumnMetamodel(name, caption, width, widthMode, property.ReturnType, filterMetamodel);
        }
    }
}