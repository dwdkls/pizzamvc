using System;

namespace Pizza.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UniqueAttribute : Attribute
    {
    }
}