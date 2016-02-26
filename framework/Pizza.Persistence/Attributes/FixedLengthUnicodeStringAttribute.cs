namespace Pizza.Persistence.Attributes
{
    public sealed class FixedLengthUnicodeStringAttribute : StringAttribute
    {
        public FixedLengthUnicodeStringAttribute(int length) : base(length) { }
    }
}