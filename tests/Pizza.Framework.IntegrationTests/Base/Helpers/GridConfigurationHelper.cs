using System;
using System.Linq.Expressions;
using Pizza.Contracts.Presentation;
using Pizza.Contracts.Presentation.Operations.Requests.Configuration;
using Pizza.Framework.Utils;

namespace Pizza.Framework.IntegrationTests.Base.Helpers
{
    public class GridConfigurationHelper
    {
        public static FilterConfiguration<TGridModel> GetFilter<TGridModel, TValue>(
                Expression<Func<TGridModel, TValue>> property, TValue value)
            where TGridModel : IGridModelBase
        {
            if (property == null)
            {
                return FilterConfiguration<TGridModel>.Empty;
            }

            var condition = GetFilterCondition(property, value.ToString());
            return new FilterConfiguration<TGridModel>(condition);
        }

        private static FilterCondition GetFilterCondition<TGridModel, TValue>(
            Expression<Func<TGridModel, TValue>> property, string value) 
            where TGridModel : IGridModelBase
        {
            var propertyName = ObjectHelper.GetPropertyName(property);
            var propertyType = ObjectHelper.GetPropertyType(property);
            var filterOperator = GetFilterOperator(propertyType);

            return new FilterCondition(propertyName, filterOperator, propertyType, value);
        }

        private static FilterOperator GetFilterOperator(Type propertyType)
        {
            FilterOperator filterOperator;
            if (propertyType == typeof(string))
            {
                filterOperator = FilterOperator.Like;
            }
            else if (propertyType == typeof(DateTime))
            {
                filterOperator = FilterOperator.DateEquals;
            }
            else
            {
                filterOperator = FilterOperator.Select;
            }
            return filterOperator;
        }

        public static SortConfiguration GetSort<TGridModel>(
            Expression<Func<TGridModel, object>> property, SortMode sortMode = SortMode.Ascending)
        {
            var propertyName = ObjectHelper.GetPropertyName(property);
            return new SortConfiguration(propertyName, sortMode);
        }
    }
}