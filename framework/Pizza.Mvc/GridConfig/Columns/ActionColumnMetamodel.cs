namespace Pizza.Mvc.GridConfig.Columns
{
    public sealed class ActionColumnMetamodel : ColumnMetamodelBase
    {
        public string ControllerName { get; private set; }
        public string ActionName { get; private set; }

        public ActionColumnMetamodel(string controllerName, string actionName, string name, string caption, int width, ColumnWidthMode widthMode)
            : base(name, caption, width, widthMode)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
        }
    }
}