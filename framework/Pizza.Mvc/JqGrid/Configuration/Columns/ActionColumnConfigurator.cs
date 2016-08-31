using System.Web.Mvc;
using System.Web.Mvc.Html;
using MvcJqGrid;
using Pizza.Mvc.GridConfig.Columns;

namespace Pizza.Mvc.JqGrid.Configuration.Columns
{
    internal sealed class ActionColumnConfigurator : GridColumnConfiguratorBase<ActionColumnMetamodel>
    {
        private readonly HtmlHelper html;

        public ActionColumnConfigurator(HtmlHelper html)
        {
            this.html = html;
        }

        protected override Column InternalRender(ActionColumnMetamodel metamodel)
        {
            var column = new Column(metamodel.Name)
                .SetLabel(metamodel.Caption)
                .SetWidth(metamodel.Width)
                .SetFixedWidth(true)
                .SetSortable(false)
                .SetSearch(false)
                .SetFixedWidth(metamodel.IsFixedWidth);

            // TODO: probably here is place where other than row item id can be placed in link
            var linkTemplate = this.html.ActionLink(metamodel.Caption, metamodel.ActionName, new { controller = metamodel.ControllerName, id = "_id_" });
            var linkFormatter = string.Format(
                @"function buildActionLink(cellvalue, options, rowobject) {{ return '{0}'.replace('_id_', options.rowId); }}",
                linkTemplate);

            column.SetCustomFormatter(linkFormatter);

            return column;
        }
    }
}