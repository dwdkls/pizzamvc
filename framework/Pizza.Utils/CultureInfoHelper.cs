using System;
using System.Globalization;

namespace Pizza.Utils
{
    public static class CultureInfoHelper
    {
        public static CultureInfo CurrentDateTimeCulture
        {
            get { return CultureInfo.CurrentCulture.Name == "pl-PL" ? new CultureInfo("de-DE") : CultureInfo.CurrentCulture; }
        }

        public static string ShortDatePattern
        {
            get { return CurrentDateTimeCulture.DateTimeFormat.ShortDatePattern; }
        }

        public static CultureInfo GetCultureInfoForType(Type type)
        {
            return type.IsReallyDateTime() ? CultureInfoHelper.CurrentDateTimeCulture : CultureInfo.CurrentCulture;
        }
    }
}
