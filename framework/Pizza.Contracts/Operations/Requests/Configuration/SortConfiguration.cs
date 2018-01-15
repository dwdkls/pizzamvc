namespace Pizza.Contracts.Operations.Requests.Configuration
{
    public sealed class SortConfiguration
    {
        public string PropertyName { get; }
        public SortMode Mode { get; }

        public SortConfiguration(string propertyName, SortMode mode)
        {
            this.PropertyName = propertyName;
            this.Mode = mode;
        }
    }
}