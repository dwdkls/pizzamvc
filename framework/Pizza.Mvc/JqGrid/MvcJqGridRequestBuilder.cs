using System.Collections.Generic;
using System.Linq;
using MvcJqGrid;
using Pizza.Contracts;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.GridConfig;
using Pizza.Mvc.GridConfig.Columns;

namespace Pizza.Mvc.JqGrid
{
    public static class MvcJqGridRequestBuilder
    {
        public static DataRequest<TGridModel> BuildGridDataRequest<TGridModel>(
            GridSettings gridSettings, GridMetamodel<TGridModel> gridMetamodel)
            where TGridModel : IGridModelBase
        {
            var sortSettings = PrepareSortSettings(gridSettings, gridMetamodel.DefaultSortSettings);
            var searchSettings = FillSearchFilter(gridMetamodel, gridSettings);
            var request = new DataRequest<TGridModel>(gridSettings.PageIndex, gridSettings.PageSize, sortSettings, searchSettings);

            return request;
        }

        private static SortConfiguration PrepareSortSettings(
            GridSettings gridSettings, SortConfiguration defaultSortSettings)
        {
            if (string.IsNullOrEmpty(gridSettings.SortColumn))
            {
                return defaultSortSettings;
            }

            var sortMode = gridSettings.SortOrder == "asc" ? SortMode.Ascending : SortMode.Descending;
            return new SortConfiguration(gridSettings.SortColumn, sortMode);
        }

        private static FilterConfiguration<TGridModel> FillSearchFilter<TGridModel>(
            GridMetamodel<TGridModel> gridMetamodel, GridSettings gridSettings)
            where TGridModel : IGridModelBase
        {
            var conditions = new List<FilterCondition>();
            if (gridSettings.IsSearch)
            {
                foreach (var rule in gridSettings.Where.rules)
                {
                    var propertiesColumns = gridMetamodel.Columns.OfType<PropertyColumnMetamodel>();
                    var column = propertiesColumns.Single(c => c.Name == rule.field);
                    var condition = new FilterCondition(column.Name, column.Filter.Operator, column.PropertyType, rule.data);
                    conditions.Add(condition);
                }
            }

            return new FilterConfiguration<TGridModel>(conditions);
        }
    }
}