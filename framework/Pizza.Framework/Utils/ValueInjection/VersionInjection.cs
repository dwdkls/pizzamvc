using System;
using System.Linq;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Pizza.Persistence;

namespace Pizza.Framework.Utils.ValueInjection
{
    public class VersionInjection : IValueInjection
    {
        private static readonly string versionPropertyName = ObjectHelper.GetPropertyName<IVersionable>(x => x.Version);

        public object Map(object source, object target)
        {
            var spd = source.GetProps().SingleOrDefault(p => p.Name == versionPropertyName);
            var tpd = target.GetProps().SingleOrDefault(p => p.Name == versionPropertyName);

            if (spd == null || tpd == null) 
                return target;

            if (spd.PropertyType == typeof(byte[]) && tpd.PropertyType == typeof(string))
            {
                byte[] val = (byte[])spd.GetValue(source);
                if (val != null)
                {
                    tpd.SetValue(target, Convert.ToBase64String(val));
                }
            }
            else if (spd.PropertyType == typeof(string) && tpd.PropertyType == typeof(byte[]))
            {
                string val = (string)spd.GetValue(source);
                if (val != null)
                {
                    tpd.SetValue(target, Convert.FromBase64String(val));
                }
            }

            return target;
        }
    }
}