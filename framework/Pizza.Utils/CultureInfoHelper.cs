using System.Globalization;

namespace Pizza.Utils
{
    public static class CultureInfoHelper
    {
        public static CultureInfo CurrentCultureForDateTimeConversion
        {
            get { return CultureInfo.CurrentCulture.Name == "pl-PL" ? new CultureInfo("de-DE") : CultureInfo.CurrentCulture; }
        }
    }
}
