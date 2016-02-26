using System;
using System.Linq.Expressions;
using NHibernate;
using Pizza.Contracts.Persistence;
using Pizza.Contracts.Presentation;
using Pizza.Contracts.Presentation.Operations;
using Pizza.Contracts.Presentation.Operations.Requests;
using Pizza.Contracts.Presentation.Operations.Results;
using Pizza.Framework.Persistence.Transactions;

namespace Pizza.Framework.Operations
{
    [Transactional]
    public abstract class GridServiceBase<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel>
        : IGridServiceBase<TGridModel, TDetailsModel, TEditModel, TCreateModel>
        where TPersistenceModel : IPersistenceModel, new()
        where TGridModel : IGridModelBase, new()
        where TDetailsModel : IDetailsModelBase, new()
        where TEditModel : IEditModelBase, new()
        where TCreateModel : ICreateModelBase, new()
    {
        protected readonly ISession session;
        protected readonly PersistenceModelsStore<TPersistenceModel, TEditModel, TCreateModel> persistenceModelsStore;
        protected readonly ViewModelsProvider<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel> viewModelsProvider;

        protected GridServiceBase(ISession session)
        {
            this.session = session;
            this.persistenceModelsStore = new PersistenceModelsStore<TPersistenceModel, TEditModel, TCreateModel>(session);
            this.viewModelsProvider = new ViewModelsProvider<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel>(session);
        }

        public DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request)
        {
            return this.viewModelsProvider.GetDataPage(request, null, null);
        }

        protected DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request,
            Expression<Func<TPersistenceModel, bool>> whereCondition)
        {
            return this.viewModelsProvider.GetDataPage(request, whereCondition, null);
        }

        public virtual TCreateModel GetCreateModel()
        {
            return this.viewModelsProvider.GetCreateModel();
        }

        public virtual TEditModel GetEditModel(int id)
        {
            return this.viewModelsProvider.GetEditModel(id);
        }

        public virtual TDetailsModel GetDetailsModel(int id)
        {
            return this.viewModelsProvider.GetDetailsModel(id);
        }

        public virtual int Create(TCreateModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException("createModel");
            }

            return this.persistenceModelsStore.Create(createModel);
        }

        public virtual void Update(TEditModel editModel)
        {
            if (editModel == null)
            {
                throw new ArgumentNullException("editModel");
            }

            this.persistenceModelsStore.Update(editModel);
        }

        public virtual void Delete(int id)
        {
            this.persistenceModelsStore.Delete(id);
        }
    }
}