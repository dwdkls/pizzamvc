namespace Pizza.Mvc.GridConfig.Columns
{
    public abstract class ColumnMetamodelBase
    {
        public string Name { get; }
        public string Caption { get; }
        public int Width { get; }
        public bool IsFixedWidth { get; }

        protected ColumnMetamodelBase(string name, string caption, int width, ColumnWidthMode widthMode)
        {
            this.Name = name;
            this.Caption = caption;
            this.Width = width;
            this.IsFixedWidth = widthMode == ColumnWidthMode.Fixed;
        }
    }
}