using System;

namespace Pizza.Persistence.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class StringAttribute : Attribute
    {
        public const int MaxLength = 9999;

        public int Length { get; private set; }

        protected StringAttribute(int maxLenght)
        {
            this.Length = maxLenght;
        }
    }
}