using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pizza.Utils
{
    public static class ReflectionExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            return (TAttribute)memberInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
        }

        public static Dictionary<PropertyInfo, TAttribute> GetAllPropertiesWithAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            var result = new Dictionary<PropertyInfo, TAttribute>();
            foreach (var propertyInfo in type.GetProperties())
            {
                var attribute = propertyInfo.GetAttribute<TAttribute>();
                if (attribute != null)
                {
                    result.Add(propertyInfo, attribute);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns real (not nullable) type of property.
        /// </summary>
        public static Type GetRealType(this Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static bool IsReallyDateTime(this Type type)
        {
            return type.GetRealType() == typeof(DateTime);
        }

        public static string GetFullMethodName(this MethodInfo methodInfo)
        {
            return $"{methodInfo.DeclaringType.Name}.{methodInfo.Name}";
        }
    }
}
