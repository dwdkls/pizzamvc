using System;

namespace Pizza.Framework.Operations.Requests.Configuration
{
    public sealed class FilterCondition
    {
        public string PropertyName { get; private set; }
        public FilterOperator Operator { get; private set; }
        public object Value { get; private set; }

        public FilterCondition(string propertyName, FilterOperator filterOperator, Type propertyType, string valueAsString)
        {
            this.PropertyName = propertyName;
            this.Operator = filterOperator;
            this.Value = GetValue(propertyType, valueAsString);
        }

        private static object GetValue(Type propertyType, string valueAsString)
        {
            if (propertyType.IsEnum)
            {
                return Enum.Parse(propertyType, valueAsString);
            }

            var value = Convert.ChangeType(valueAsString, propertyType);
            return value;
        }
    }
}