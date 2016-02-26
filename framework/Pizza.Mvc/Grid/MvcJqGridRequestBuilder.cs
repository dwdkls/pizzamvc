using System.Collections.Generic;
using System.Linq;
using MvcJqGrid;
using Pizza.Contracts;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.Grid.Metamodel;

namespace Pizza.Mvc.Grid
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
                    var column = gridMetamodel.Columns.Single(c => c.PropertyName == rule.field);
                    var condition = new FilterCondition(column.PropertyName, column.Filter.Operator, column.PropertyType, rule.data);
                    conditions.Add(condition);
                }
            }

            return new FilterConfiguration<TGridModel>(conditions);
        }
    }
}