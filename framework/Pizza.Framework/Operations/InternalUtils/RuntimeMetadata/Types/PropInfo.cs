using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Pizza.Utils;

namespace Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types
{
    [DebuggerDisplay("Name: {Name} TypeName: {Type.Name}")]
    public class PropInfo
    {
        public Type Type { get; private set; }
        public string Name { get; private set; }

        public PropInfo(string name, Type type)
        {
            this.Name = name;
            this.Type = type;
        }

        public static PropInfo FromPropertyInfo(PropertyInfo propertyInfo)
        {
            return new PropInfo(propertyInfo.Name, propertyInfo.PropertyType);
        }

        public static PropInfo FromPropertyExpression<T>(Expression<Func<T, object>> expression)
        {
            var name = ObjectHelper.GetPropertyName(expression);
            var type = ObjectHelper.GetPropertyType(expression);

            return new PropInfo(name, type);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            var other = (PropInfo)obj;
            return this.Name == other.Name && this.Type == other.Type;

        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode() ^ this.Type.GetHashCode();
        }
    }
}