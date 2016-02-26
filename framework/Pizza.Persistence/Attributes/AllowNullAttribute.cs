using System;

namespace Pizza.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowNullAttribute : Attribute
    {
    }
}