using System;
using System.Collections.Generic;
using MvcJqGrid;
using MvcJqGrid.Enums;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.GridConfig;
using Pizza.Mvc.GridConfig.Columns;

namespace Pizza.Mvc.JqGrid.Configuration.Columns
{
    internal sealed class PropertyColumnConfigurator : GridColumnConfiguratorBase<PropertyColumnMetamodel>
    {
        protected override Column InternalRender(PropertyColumnMetamodel metamodel)
        {
            var column = new Column(metamodel.Name)
                .SetLabel(metamodel.Caption)
                .SetWidth(metamodel.Width)
                .SetFixedWidth(metamodel.IsFixedWidth);

            ConfigureSearch(column, metamodel.Filter);

            return column;
        }

        private static void ConfigureSearch(Column column, FilterMetamodel filter)
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
            }
        }

        private static readonly Dictionary<FilterOperator, Searchtype> filterToSearchtypeMap = new Dictionary<FilterOperator, Searchtype> {
            { FilterOperator.DateEquals, Searchtype.Datepicker },
            { FilterOperator.Select, Searchtype.Select },
            { FilterOperator.Like, Searchtype.Text },
        };
    }
}