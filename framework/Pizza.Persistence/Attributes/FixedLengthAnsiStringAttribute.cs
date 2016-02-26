namespace Pizza.Persistence.Attributes
{
    public sealed class FixedLengthAnsiStringAttribute : StringAttribute
    {
        public FixedLengthAnsiStringAttribute(int length) : base(length) { }
    }
}