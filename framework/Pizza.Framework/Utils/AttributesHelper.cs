using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Pizza.Framework.Utils
{
    public class AttributesHelper
    {
        public static string GetPropertyDisplayName<TObject>(Expression<Func<TObject, object>> propertyExpression)
           where TObject : new()
        {
            return GetPropertyAttributeValue<TObject, DisplayAttribute, string>(propertyExpression, a => a.Name);
        }

        public static string GetPropertyDisplayName(LambdaExpression propertyExpression)
        {
            return GetPropertyAttributeValue<DisplayAttribute, string>(propertyExpression, a => a.Name);
        }


        public static TResult GetPropertyAttributeValue<TObject, TAttribute, TResult>(
            Expression<Func<TObject, object>> propertyExpression, Expression<Func<TAttribute, TResult>> attributeExpression)
            where TAttribute : Attribute
            where TObject : new()
            where TResult : class
        {
            return GetPropertyAttributeValue((LambdaExpression)propertyExpression, attributeExpression);
        }

        public static TResult GetPropertyAttributeValue<TAttribute, TResult>(
            LambdaExpression propertyExpression, Expression<Func<TAttribute, TResult>> attributeExpression)
            where TAttribute : Attribute
            where TResult : class
        {
            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                body = ((UnaryExpression)propertyExpression.Body).Operand as MemberExpression;
            }

            if (body == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var attribute = body.Member.GetAttribute<TAttribute>();
            if (attribute == null)
            {
                return null;
                //throw new ArgumentNullException("attributeExpression");
            }

            var result = attributeExpression.Compile().Invoke(attribute);
            return result;
        }
    }
}
