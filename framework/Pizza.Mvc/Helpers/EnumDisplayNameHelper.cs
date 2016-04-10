using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pizza.Contracts.Attributes;
using Pizza.Utils;

namespace Pizza.Mvc.Helpers
{
    public static class EnumDisplayNameHelper
    {
        public static Dictionary<string, string> GetValueAndDescriptionMap(Type enumType)
        {
            var namesForValuesList = new Dictionary<string, string>();

            var enumMembers = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static);
            foreach (var enumMember in enumMembers)
            {
                var enumAttribute = enumMember.GetAttribute<EnumDisplayNameAttribute>();
                if (enumAttribute != null)
                {
                    namesForValuesList.Add(enumMember.Name, enumAttribute.Name);
                }
            }

            return namesForValuesList;
        }

        public static Dictionary<int, string> GetValueNameMap(Type enumType)
        {
            enumType = enumType.GetRealType();

            var nameValueMapFromEnum = new Dictionary<int, string>();
            var enumMembers = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static);
            var enumValues = Enum.GetValues(enumType);
            foreach (var value in enumValues)
            {
                var enumMember = enumMembers.SingleOrDefault(x => x.Name == value.ToString());
                var enumAttribute = enumMember.GetAttribute<EnumDisplayNameAttribute>();

                if (enumAttribute != null)
                {
                    nameValueMapFromEnum.Add((int)value, enumAttribute.Name);
                }
            }

            return nameValueMapFromEnum;
        }

        public static string GetDisplayName(Type enumType, object enumValue)
        {
            var enumMembers = enumType.GetMembers(BindingFlags.Public | BindingFlags.Static); // TODO: create GetPublicStaticMembers method?
            var enumMember = enumMembers.SingleOrDefault(x => x.Name == enumValue.ToString());
            var enumAttribute = enumMember.GetAttribute<EnumDisplayNameAttribute>();

            if (enumAttribute != null)
            {
                return enumAttribute.Name;
            }

            return null;
        }
    }
}