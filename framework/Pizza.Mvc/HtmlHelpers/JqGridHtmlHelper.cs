using MvcJqGrid;
using MvcJqGrid.Enums;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.Grid.Metamodel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Pizza.Mvc.HtmlHelpers
{
    public static class JqGridHtmlHelper
    {
        public static MvcHtmlString JqGrid(this HtmlHelper html, GridMetamodel gridModel, string gridDataAction)
        {
            // TODO: fluent API
            var grid = html.Grid("mainGrid");
            grid = ConfigureGrid(grid, gridModel, gridDataAction);
            grid = ConfigureDataColumns(grid, gridModel);
            grid = ConfigureButtonColumns(grid, gridModel);

            return MvcHtmlString.Create(grid.ToString());
        }

        private static MvcJqGrid.Grid ConfigureGrid(MvcJqGrid.Grid grid, GridMetamodel gridModel, string gridDataAction)
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
                .OnLoadError("showErrorWindow(xhr.responseText, false);");

            return grid;
        }

        private static MvcJqGrid.Grid ConfigureDataColumns(MvcJqGrid.Grid grid, GridMetamodel gridModel)
        {
            foreach (var metamodel in gridModel.Columns)
            {
                var column = new Column(metamodel.PropertyName)
                    .SetLabel(metamodel.DisplayName)
                    .SetWidth(metamodel.Width)
                    .SetFixedWidth(metamodel.IsFixedWidth);

                column.ConfigureSearch(metamodel.Filter);

                grid.AddColumn(column);
            }

            return grid;
        }

        private static void ConfigureSearch(this Column column, FilterMetamodel filter)
        {
            var filterOperator = filter.Operator;
            if (filterOperator == FilterOperator.Auto)
            {
                throw new ApplicationException("Invalid FilterOperator value on this level. This should be resolved and replaced during GridMetaModel building.");
            }

            if (filterOperator == FilterOperator.Disabled)
            {
                column.SetSearch(false);
            }
            else
            {
                var searchtype = filterToSearchtypeMap[filterOperator];

                column.SetSearchType(searchtype);

                if (searchtype == Searchtype.Select)
                {
                    column.SetSearchTerms(filter.SelectFilterMap);
                }
                else if (searchtype == Searchtype.Datepicker)
                {
                    column.SetSearchDateFormat("yy-mm-dd");
                }
            }
        }

        private static readonly Dictionary<FilterOperator, Searchtype> filterToSearchtypeMap = new Dictionary<FilterOperator, Searchtype> {
            { FilterOperator.DateEquals, Searchtype.Datepicker },
            { FilterOperator.Select, Searchtype.Select },
            { FilterOperator.Like, Searchtype.Text },
        };

        private static MvcJqGrid.Grid ConfigureButtonColumns(MvcJqGrid.Grid grid, GridMetamodel gridModel)
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

            return grid;
        }
    }
}