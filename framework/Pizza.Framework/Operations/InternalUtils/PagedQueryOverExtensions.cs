using System;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using Pizza.Contracts;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Persistence;

namespace Pizza.Framework.Operations.InternalUtils
{
    internal static class PagedQueryOverExtensions
    {
        public static QueryOver<TPersistenceModel, TPersistenceModel> AddAlliasesForAllSubpropertiesInViewModel<TPersistenceModel>(
                this QueryOver<TPersistenceModel, TPersistenceModel> query, PersistenceModelPropertiesDescription modelDescription)
            where TPersistenceModel : IPersistenceModel
        {
            foreach (var joinedModel in modelDescription.JoinedModels)
            {
                query.UnderlyingCriteria.CreateAlias(joinedModel.Name, joinedModel.Name, JoinType.InnerJoin);
            }

            return query;
        }

        public static QueryOver<TPersistenceModel, TPersistenceModel> ApplyFilter<TPersistenceModel, TGridModel>(
                this QueryOver<TPersistenceModel, TPersistenceModel> query,
                FilterConfiguration<TGridModel> filterConfiguration,
                ViewModelToPersistenceModelPropertyNamesMaps viewModelToPersistenceModelMap)
            where TPersistenceModel : IPersistenceModel
            where TGridModel : IGridModelBase
        {
            foreach (var condition in filterConfiguration.Conditions)
            {
                string conditionPropertyName = viewModelToPersistenceModelMap.AllProperties[condition.PropertyName];

                switch (condition.Operator)
                {
                    case FilterOperator.Select:
                        query.UnderlyingCriteria.Add(Restrictions.Eq(conditionPropertyName, condition.Value));
                        break;

                    case FilterOperator.Like:
                        query.UnderlyingCriteria.Add(Restrictions.Like(conditionPropertyName, String.Format("%{0}%", condition.Value)));
                        break;

                    case FilterOperator.DateEquals:
                        var initDate = (DateTime)condition.Value;
                        var endDate = initDate.AddDays(1).AddSeconds(-1);
                        query.UnderlyingCriteria.Add(Restrictions.Between(conditionPropertyName, initDate, endDate));
                        break;
                }
            }

            return query;
        }

        public static QueryOver<TPersistenceModel, TPersistenceModel> ApplyOrder<TPersistenceModel>(
                this QueryOver<TPersistenceModel, TPersistenceModel> query,
                SortConfiguration sortSettings, ViewModelToPersistenceModelPropertyNamesMaps viewModelToPersistenceModelMap)
            where TPersistenceModel : IPersistenceModel
        {
            bool ascending = sortSettings.Mode == SortMode.Ascending;
            string sortPropertyName = viewModelToPersistenceModelMap.AllProperties[sortSettings.PropertyName];
            query.UnderlyingCriteria.AddOrder(new Order(sortPropertyName, ascending));

            return query;
        }

        public static ICriteria ProjectToViewModel<TPersistenceModel, TGridModel>(
                this IQueryOver<TPersistenceModel, TPersistenceModel> query, ProjectionList projectionsList)
        {
            query.UnderlyingCriteria
                .SetProjection(projectionsList)
                .SetResultTransformer(Transformers.AliasToBean<TGridModel>());

            return query.UnderlyingCriteria;
        }
    }
}