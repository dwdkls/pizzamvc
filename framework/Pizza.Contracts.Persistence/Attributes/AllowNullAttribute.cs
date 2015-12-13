using System;

namespace Pizza.Contracts.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowNullAttribute : Attribute
    {
    }
}