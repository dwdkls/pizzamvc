namespace Pizza.Contracts.Presentation.Operations.Requests.Configuration
{
    public sealed class SortConfiguration
    {
        public string PropertyName { get; private set; }
        public SortMode Mode { get; private set; }

        public SortConfiguration(string propertyName, SortMode mode)
        {
            this.PropertyName = propertyName;
            this.Mode = mode;
        }
    }
}