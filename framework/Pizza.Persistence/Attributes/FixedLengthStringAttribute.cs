namespace Pizza.Persistence.Attributes
{
    public sealed class FixedLengthStringAttribute : StringAttribute
    {
        public FixedLengthStringAttribute(int maxLenght) : base(maxLenght) { }
    }
}