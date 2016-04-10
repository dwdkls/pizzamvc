using System;
using System.Linq.Expressions;
using Pizza.Utils;

namespace Pizza.Mvc.Grid.Metamodel
{
    public class ColumnMetamodel
    {
        public string PropertyName { get; private set; }
        public string DisplayName { get; private set; }
        public Type PropertyType { get; private set; }
        public int Width { get; private set; }
        public bool IsFixedWidth { get; private set; }
        public FilterMetamodel Filter { get; private set; }

        public ColumnMetamodel(LambdaExpression property, int width, ColumnWidthMode widthMode, FilterMetamodel filterMetamodel)
        {
            this.PropertyName = ObjectHelper.GetPropertyName(property);

            var displayName = AttributesHelper.GetPropertyDisplayName(property);
            this.DisplayName = displayName ?? property.Name;

            this.PropertyType = property.ReturnType;
            this.Width = width;
            this.IsFixedWidth = widthMode == ColumnWidthMode.Fixed;
            this.Filter = filterMetamodel;
        }
    }
}