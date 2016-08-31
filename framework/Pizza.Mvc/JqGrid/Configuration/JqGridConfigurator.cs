using System;
using System.Web.Mvc;
using MvcJqGrid;
using Pizza.Mvc.GridConfig;
using Pizza.Mvc.GridConfig.Columns;
using Pizza.Mvc.JqGrid.Configuration.Columns;

namespace Pizza.Mvc.JqGrid.Configuration
{
    internal class JqGridConfigurator
    {
        private readonly HtmlHelper html;

        public JqGridConfigurator(HtmlHelper html)
        {
            this.html = html;
        }

        public Grid Configure(GridMetamodel gridModel, string gridDataAction, string gridId)
        {
            var grid = this.html.Grid(gridId);
            ConfigureGrid(grid, gridModel, gridDataAction);
            this.ConfigureModelColumns(grid, gridModel);
            ConfigureCrudButtons(grid, gridModel);

            return grid;
        }

        private static void ConfigureGrid(Grid grid, GridMetamodel gridModel, string gridDataAction)
        {
            grid
                // basic:
                .SetCaption(gridModel.Caption)
                .SetUrl(gridDataAction)
                .SetHideGrid(false)
                .SetAutoWidth(true)
                // pagination:        
                .SetPager("pager")
                .SetTopPager(true)
                .SetRowList(new[] {
                    10, 20, 50, 100
                })
                .SetViewRecords(true)
                // sorting and filtering:
                .SetShowAllSortIcons(true)
                .SetSearchToolbar(true)
                .SetSearchOnEnter(true)
                .SetSearchClearButton(true)
                // events:
                .OnGridComplete("gridComplete()")
                .OnLoadError("showGridError(xhr.responseText);");
        }

        private void ConfigureModelColumns(Grid grid, GridMetamodel gridModel)
        {
            foreach (var metamodel in gridModel.Columns)
            {
                var metamodelType = metamodel.GetType();
                var configurator = this.GetConfigurator(metamodelType);
                var column = configurator.Render(metamodel);

                grid.AddColumn(column);
            }
        }

        private static void ConfigureCrudButtons(Grid grid, GridMetamodel gridModel)
        {
            if (gridModel.DetailsLink.IsEnabled)
            {
                grid.AddColumn(new Column("DetailsLink")
                    .SetLabel(gridModel.DetailsLink.Text)
                    .SetWidth(70).SetFixedWidth(true)
                    .SetSortable(false)
                    .SetSearch(false)
                    .SetCustomFormatter("buildDetailsLink"));
            }

            if (gridModel.EditLink.IsEnabled)
            {
                grid.AddColumn(new Column("EditLink")
                    .SetLabel(gridModel.EditLink.Text)
                    .SetWidth(50).SetFixedWidth(true)
                    .SetSortable(false)
                    .SetSearch(false)
                    .SetCustomFormatter("buildEditLink"));
            }

            if (gridModel.DeleteLink.IsEnabled)
            {
                grid.AddColumn(new Column("DeleteLink")
                    .SetLabel(gridModel.DeleteLink.Text)
                    .SetWidth(50).SetFixedWidth(true)
                    .SetSortable(false)
                    .SetSearch(false)
                    .SetCustomFormatter("buildDeleteLink"));
            }
        }

        private GridColumnConfiguratorBase GetConfigurator(Type metamodelType)
        {
            if (metamodelType == typeof(ActionColumnMetamodel))
            {
                return new ActionColumnConfigurator(this.html);
            }
            if (metamodelType == typeof(PropertyColumnMetamodel))
            {
                return new PropertyColumnConfigurator();
            }

            throw new ArgumentOutOfRangeException("metamodelType");
        }
    }
}