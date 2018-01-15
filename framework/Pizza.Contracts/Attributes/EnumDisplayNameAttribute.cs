using System;

namespace Pizza.Contracts.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : Attribute
    {
        public string Name { get; }

        public EnumDisplayNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}