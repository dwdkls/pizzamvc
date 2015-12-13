using System;

namespace Pizza.Contracts.Presentation.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayNameAttribute : Attribute
    {
        public string Name { get; private set; }

        public EnumDisplayNameAttribute(string name)
        {
            this.Name = name;
        }
    }
}