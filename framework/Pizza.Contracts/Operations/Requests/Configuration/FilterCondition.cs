using System;
using System.Globalization;
using Pizza.Utils;

namespace Pizza.Contracts.Operations.Requests.Configuration
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
            var realType = propertyType.GetRealType();
            if (realType.IsEnum)
            {
                return Enum.Parse(propertyType, valueAsString);
            }

            var conversionCulture = realType == typeof(DateTime) ? CultureInfoHelper.CurrentCultureForDateTimeConversion : CultureInfo.CurrentCulture;

            var value = Convert.ChangeType(valueAsString, propertyType, conversionCulture);
            return value;
        }
    }
}