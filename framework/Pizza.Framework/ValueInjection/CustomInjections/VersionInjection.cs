using System;
using System.Linq;
using Omu.ValueInjecter.Injections;
using Omu.ValueInjecter.Utils;
using Pizza.Persistence;
using Pizza.Utils;

namespace Pizza.Framework.ValueInjection.CustomInjections
{
    internal class VersionInjection : IValueInjection
    {
        private static readonly string versionPropertyName = nameof(IVersionable.Version);

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