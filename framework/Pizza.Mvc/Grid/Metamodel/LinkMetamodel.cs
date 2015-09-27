namespace Pizza.Mvc.Grid.Metamodel
{
    public class LinkMetamodel
    {
        public bool IsEnabled { get; private set; }
        public string Text { get; private set; }

        public LinkMetamodel(bool isEnabled, string text)
        {
            this.IsEnabled = isEnabled;
            this.Text = text;
        }

        public static LinkMetamodel Disabled
        {
            get { return new LinkMetamodel(false, string.Empty); }
        }
    }
}