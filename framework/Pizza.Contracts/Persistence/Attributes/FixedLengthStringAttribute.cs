namespace Pizza.Contracts.Persistence.Attributes
{
    public sealed class FixedLengthStringAttribute : StringAttribute
    {
        public FixedLengthStringAttribute(int maxLenght) : base(maxLenght) { }
    }
}