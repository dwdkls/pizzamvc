using System;
using Pizza.Utils;

namespace Pizza.Contracts.Operations.Requests.Configuration
{
    public sealed class FilterCondition
    {
        public string PropertyName { get; }
        public FilterOperator Operator { get; }
        public object Value { get; }

        public FilterCondition(string propertyName, FilterOperator filterOperator, Type propertyType, string valueAsString)
        {
            this.PropertyName = propertyName;
            this.Operator = filterOperator;
            this.Value = GetValue(propertyType, valueAsString);
        }

        private static object GetValue(Type propertyType, string valueAsString)
        {
            var realType = propertyType.GetRealType();

            if (realType.IsEnum)
            {
                return Enum.Parse(realType, valueAsString);
            }

            var cultureInfo = CultureInfoHelper.GetCultureInfoForType(realType);
            return Convert.ChangeType(valueAsString, propertyType, cultureInfo);
        }
    }
}