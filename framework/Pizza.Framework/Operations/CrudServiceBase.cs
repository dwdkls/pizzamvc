using System;
using System.Linq.Expressions;
using NHibernate;
using Pizza.Contracts;
using Pizza.Contracts.Operations;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Persistence;

namespace Pizza.Framework.Operations
{
    [Transactional]
    public abstract class CrudServiceBase<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel>
        : ICrudServiceBase<TGridModel, TDetailsModel, TEditModel, TCreateModel>
        where TPersistenceModel : IPersistenceModel, new()
        where TGridModel : IGridModelBase, new()
        where TDetailsModel : IDetailsModelBase, new()
        where TEditModel : IEditModelBase, new()
        where TCreateModel : ICreateModelBase, new()
    {
        protected readonly ISession session;
        protected readonly PersistenceModelsStore<TPersistenceModel, TEditModel, TCreateModel> persistenceModelsStore;
        protected readonly ViewModelsReader<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel> viewModelsReader;

        protected CrudServiceBase(ISession session)
        {
            this.session = session;
            this.persistenceModelsStore = new PersistenceModelsStore<TPersistenceModel, TEditModel, TCreateModel>(session);
            this.viewModelsReader = new ViewModelsReader<TPersistenceModel, TGridModel, TDetailsModel, TEditModel, TCreateModel>(session);
        }

        public virtual DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request)
        {
            return this.viewModelsReader.GetDataPage(request, null, null);
        }

        protected virtual DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request,
            Expression<Func<TPersistenceModel, bool>> whereCondition)
        {
            return this.viewModelsReader.GetDataPage(request, whereCondition, null);
        }

        public virtual CrudOperationResult<TCreateModel> GetCreateModel()
        {
            return this.viewModelsReader.GetCreateModel();
        }

        public virtual CrudOperationResult<TEditModel> GetEditModel(int id)
        {
            return this.viewModelsReader.GetEditModel(id);
        }

        public virtual CrudOperationResult<TDetailsModel> GetDetailsModel(int id)
        {
            return this.viewModelsReader.GetDetailsModel(id);
        }

        public virtual CrudOperationResult<int> Create(TCreateModel createModel)
        {
            if (createModel == null)
            {
                throw new ArgumentNullException("createModel");
            }

            return this.persistenceModelsStore.Create(createModel);
        }

        public virtual CrudOperationResult Update(TEditModel editModel)
        {
            if (editModel == null)
            {
                throw new ArgumentNullException("editModel");
            }

            return this.persistenceModelsStore.Update(editModel);
        }

        public virtual CrudOperationResult Delete(int id)
        {
            return this.persistenceModelsStore.Delete(id);
        }
    }
}