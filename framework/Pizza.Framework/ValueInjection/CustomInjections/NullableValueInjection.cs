using System.Linq;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;

namespace Pizza.Framework.ValueInjection.CustomInjections
{
    internal class NullableValueInjection : IValueInjection
    {
        public object Map(object source, object target)
        {
            var sourceProps = source.GetProps();
            var targetProps = target.GetProps();

            foreach (var sProp in sourceProps)
            {
                var tProp = targetProps.SingleOrDefault(p => p.Name == sProp.Name);

                if (tProp == null) continue;

                if (sProp.PropertyType.IsGenericType && tProp.PropertyType == sProp.PropertyType.GetGenericArguments()[0]
                    || tProp.PropertyType.IsGenericType && sProp.PropertyType == tProp.PropertyType.GetGenericArguments()[0])
                {
                    var val = sProp.GetValue(source);
                    tProp.SetValue(target, val);
                }
            }

            return target;
        }
    }
}