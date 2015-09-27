namespace Pizza.Contracts.Persistence.Attributes
{
    public sealed class UnicodeStringAttribute : StringAttribute
    {
        public UnicodeStringAttribute(int maxLenght = MaxLength) : base(maxLenght) { }
    }
}