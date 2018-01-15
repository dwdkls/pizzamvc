namespace Pizza.Mvc.GridConfig.Columns
{
    public sealed class ActionColumnMetamodel : ColumnMetamodelBase
    {
        public string ControllerName { get; }
        public string ActionName { get; }

        public ActionColumnMetamodel(string controllerName, string actionName, string name, string caption, int width, ColumnWidthMode widthMode)
            : base(name, caption, width, widthMode)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
        }
    }
}