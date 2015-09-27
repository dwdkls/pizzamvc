using System;

namespace Pizza.Contracts.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class UniqueAttribute : Attribute
    {
    }
}