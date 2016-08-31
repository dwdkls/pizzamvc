using MvcJqGrid;
using Pizza.Mvc.GridConfig.Columns;

namespace Pizza.Mvc.JqGrid.Configuration.Columns
{
    internal abstract class GridColumnConfiguratorBase 
    {
        public abstract Column Render(ColumnMetamodelBase metamodel);
    }

    internal abstract class GridColumnConfiguratorBase<TColumnMetamodel> : GridColumnConfiguratorBase
       where TColumnMetamodel : ColumnMetamodelBase
    {
        public override Column Render(ColumnMetamodelBase metamodel)
        {
            var typedMetamodel = (TColumnMetamodel)metamodel;
            return this.InternalRender(typedMetamodel);
        }

        protected abstract Column InternalRender(TColumnMetamodel metamodel);
    }
}