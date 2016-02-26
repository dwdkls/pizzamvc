using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using Pizza.Contracts.Persistence;
using Pizza.Contracts.Presentation;
using Pizza.Contracts.Presentation.Operations.Requests;
using Pizza.Contracts.Presentation.Operations.Results;
using Pizza.Framework.Operations.InternalUtils;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Framework.Persistence.Extensions;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Framework.Utils.ValueInjection;

namespace Pizza.Framework.Operations
{
    [Transactional]
    public class ViewModelsProvider<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel>
        where TPersistenceModel : IPersistenceModel, new()
        where TGridModel : IGridModelBase, new()
        where TDetailsModel : IDetailsModelBase, new()
        where TEditModel : IEditModelBase, new()
        where TCreateModel : ICreateModelBase, new()
    {
        protected static readonly PersistenceModelPropertiesDescription persistenceModelDescription;
        protected static readonly ViewModelToPersistenceModelPropertyNamesMaps viewModelToPersistenceModelMap;
        protected static readonly ProjectionList projectionsList;

        protected readonly ISession session;

        public ViewModelsProvider(ISession session)
        {
            this.session = session;
        }

        static ViewModelsProvider()
        {
            persistenceModelDescription = PersistenceModelPropertiesDescriptionGenerator.GenerateDescription(typeof(TPersistenceModel));
            viewModelToPersistenceModelMap = ViewModelToPersistenceModelPropertyNamesMapsGenerator.Generate(
                typeof(TGridModel), persistenceModelDescription);
            projectionsList = ProjectionsGenerator.GenerateProjectionsList(viewModelToPersistenceModelMap);
        }

        public virtual DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request,
            Expression<Func<TPersistenceModel, bool>> whereCondition,
            Action<TGridModel, TPersistenceModel> additonalConversion)
        {
            var executableQueryOver = BuildQueryOver(this.session, request, whereCondition);

            var totalItemsCount = executableQueryOver
                .ToRowCountQuery()
                .FutureValue<int>();

            var viewModels = executableQueryOver
                .ProjectToViewModel<TPersistenceModel, TGridModel>(projectionsList)
                .Future<TGridModel>()
                .ToList();

            return new DataPageResult<TGridModel>(viewModels, request.PageNumber, request.PageSize, totalItemsCount.Value);
        }

        private static IQueryOver<TPersistenceModel, TPersistenceModel> BuildQueryOver(ISession session,
            DataRequest<TGridModel> request, Expression<Func<TPersistenceModel, bool>> whereCondition)
        {
            var dataQuery = QueryOver.Of<TPersistenceModel>()
                .AddAlliasesForAllSubpropertiesInViewModel(persistenceModelDescription)
                .ApplyFilter(request.FilterConfiguration, viewModelToPersistenceModelMap)
                .ApplyOrder(request.SortConfiguration, viewModelToPersistenceModelMap);

            if (whereCondition != null)
            {
                dataQuery.Where(whereCondition);
            }

            dataQuery.ApplyPaging(request.PageNumber, request.PageSize);

            var executableQueryOver = dataQuery.GetExecutableQueryOver(session);
            return executableQueryOver;
        }

        public virtual TDetailsModel GetDetailsModel(int id)
        {
            var persistenceModel = this.session.Load<TPersistenceModel>(id);
            var viewModel = Injector.CreateViewModelFromPersistenceModel<TPersistenceModel, TDetailsModel>(persistenceModel);
            return viewModel;
        }

        public virtual TCreateModel GetCreateModel()
        {
            return new TCreateModel();
        }

        public virtual TEditModel GetEditModel(int id)
        {
            var persistenceModel = this.session.Load<TPersistenceModel>(id);
            var editModel = Injector.CreateViewModelFromPersistenceModel<TPersistenceModel, TEditModel>(persistenceModel);
            return editModel;
        }
    }
}
