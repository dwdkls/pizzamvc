using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Pizza.Framework.Utils;
using Pizza.Mvc.Helpers;

namespace Pizza.Mvc.Grid
{
    public class TypeToJqGridObjectMapperGenerator
    {
        private static readonly Expression polishCultureExpression;
        private static readonly ConstructorInfo jqGridObjectTypeCtor;
        private static readonly MethodInfo getEnumDisplayNameMethodInfo;

        static TypeToJqGridObjectMapperGenerator()
        {
            var jqGridObjectType = new { id = -1, cell = new object[0] }.GetType();
            jqGridObjectTypeCtor = jqGridObjectType.GetConstructor(new[] { typeof(int), typeof(object[]) });

            // TODO: get from provider
            polishCultureExpression = Expression.Constant(new CultureInfo("en-US"));

            // EnumHelper.GetDisplayName
            getEnumDisplayNameMethodInfo = typeof(EnumDisplayNameHelper).GetMethod("GetDisplayName");
        }

        /// <summary>
        /// Creates function with maps any object to format required by MvcJqGrid. This function will looks like:
        /// Func<T, object> func = c => new
        /// {
        ///     id = c.Id,
        ///     cell = new[] { 
        ///        c.SomeStringProperty, Convert.ToString(c.SomeDate, polishCulture), EnumHelper.GetDisplayName(typeof(SomeEnum), c.SomeEnum)
        ///     }
        /// };
        /// </summary>
        /// <typeparam name="TSource">Source type for mapping func.</typeparam>
        /// <param name="idPropertyName">Name of id property in source type.</param>
        /// <param name="propertiesNames">Lists of sourc type properties whose will be mapped by func. The order of these properties will be preserved.</param>
        /// <returns>Mapping function.</returns>
        public static Func<TSource, object> GetMapper<TSource>(string idPropertyName, IEnumerable<string> propertiesNames)
        {
            Type sourceType = typeof(TSource);

            var sourceObjectParameter = Expression.Parameter(sourceType, "c");
            var idPointer = Expression.Property(sourceObjectParameter, sourceType.GetProperty(idPropertyName)); // c.Id
            var propertiesExpressions = propertiesNames
                .Select(sourceType.GetProperty)
                .Select(propInfo => GetPropertyExpressionForProperty(propInfo, sourceObjectParameter));     // c.Prop1, c.Prop2
            var propertiesArrayInit = Expression.NewArrayInit(typeof(object), propertiesExpressions);       // new[] { c.Prop1, c.Prop2 }

            // Now create expression: new { id = c.Id, cell = new[] { c.Prop1, c.Prop2 } } } but with constructor instead of object initialization
            var ctorExpression = Expression.New(jqGridObjectTypeCtor, idPointer, propertiesArrayInit);
            var lambda = Expression.Lambda(ctorExpression, sourceObjectParameter);
            return (Func<TSource, object>)lambda.Compile();
        }

        /// <summary>
        /// Gets property expression for some property in type. If property type is Enum or DateTime will be converted to string. 
        /// It could looks like:
        /// c.SomeString, Convert.ToString(c.SomeDate, polishCulture), EnumHelper.GetDisplayName(typeof(SomeEnum), c.SomeEnum)
        /// </summary>
        /// <param name="propertyInfo">Reflection information about property in type.</param>
        /// <param name="sourceObjectParameter">Type parameter required for property expression.</param>
        /// <returns>Property expression for requested property.</returns>
        private static Expression GetPropertyExpressionForProperty(PropertyInfo propertyInfo, ParameterExpression sourceObjectParameter)
        {
            Expression propertyExpression = Expression.Property(sourceObjectParameter, propertyInfo);

            var propertyType = propertyInfo.PropertyType.GetRealType();

            if (propertyType.IsEnum)
            {
                // Calling EnumHelper.GetDisplayName(Type enumType, object enumValue)
                var propertyTypeExpression = Expression.Constant(propertyType);
                var enumValueExpression = Expression.Convert(propertyExpression, typeof(object));
                propertyExpression = Expression.Call(getEnumDisplayNameMethodInfo, propertyTypeExpression, enumValueExpression);
            }
            else if (propertyType.IsValueType)
            {
                // Calling Convert.ToString(property type, IFormatProvider)
                var methodParmetersTypes = new[] { propertyType, typeof(IFormatProvider) };
                MethodInfo convertToStringMethod = typeof(Convert).GetMethod("ToString", methodParmetersTypes);
                propertyExpression = Expression.Call(convertToStringMethod, propertyExpression, polishCultureExpression);
            }

            return propertyExpression;
        }
    }
}