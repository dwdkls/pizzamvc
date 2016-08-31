namespace Pizza.Mvc.GridConfig.Columns
{
    public abstract class ColumnMetamodelBase
    {
        public string Name { get; private set; }
        public string Caption { get; private set; }
        public int Width { get; private set; }
        public bool IsFixedWidth { get; private set; }

        protected ColumnMetamodelBase(string name, string caption, int width, ColumnWidthMode widthMode)
        {
            this.Name = name;
            this.Caption = caption;
            this.Width = width;
            this.IsFixedWidth = widthMode == ColumnWidthMode.Fixed;
        }
    }
}