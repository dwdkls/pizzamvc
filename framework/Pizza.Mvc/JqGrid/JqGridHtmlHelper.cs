using System.Web.Mvc;
using Pizza.Mvc.GridConfig;
using Pizza.Mvc.JqGrid.Configuration;
using Pizza.Mvc.Resources;

namespace Pizza.Mvc.JqGrid
{
    public static class JqGridHtmlHelper
    {
        public static MvcHtmlString JqGrid(this HtmlHelper html, GridMetamodel gridModel, string gridDataAction)
        {
            var gridId = "mainGrid";

            var configurator = new JqGridConfigurator(html);
            var grid = configurator.Configure(gridModel, gridDataAction, gridId);

            var gridMarkup = JqGridConfigurationUpdater.FixGridConfiguration(
                grid.ToString(), gridId, UiTexts.GridButton_Clear, UiTexts.GridSearch_DropDown_All);
            return MvcHtmlString.Create(gridMarkup);
        }
    }
}