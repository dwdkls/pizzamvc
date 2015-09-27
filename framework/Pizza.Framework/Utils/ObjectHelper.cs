using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pizza.Framework.Utils
{
    public class ObjectHelper
    {
        public static string GetMethodName<T>(Expression<Action<T>> expression)
        {
            var method = (MethodCallExpression)expression.Body;
            return method.Method.Name;
        }

        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            return GetPropertyName((LambdaExpression)expression);
        }

        public static string GetPropertyName(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            return body.Member.Name;
        }

        public static Type GetPropertyType<T>(Expression<Func<T, object>> expression)
        {
            return GetPropertyType((LambdaExpression)expression);
        }

        public static Type GetPropertyType(LambdaExpression expression)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var propertyInfo = (PropertyInfo)body.Member;
            return propertyInfo.PropertyType;
        }

        public static IEnumerable<string> GetAllPropertiesNames<T>()
        {
            return typeof(T).GetProperties().Select(propertyInfo => propertyInfo.Name);
        }
    }
}
