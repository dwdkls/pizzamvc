namespace Pizza.Mvc.GridConfig
{
    // TODO: probably add Width here
    public class LinkMetamodel
    {
        public bool IsEnabled { get; }
        public string Text { get; }

        public LinkMetamodel(bool isEnabled, string text)
        {
            this.IsEnabled = isEnabled;
            this.Text = text;
        }

        public static LinkMetamodel Disabled => new LinkMetamodel(false, string.Empty);
    }
}