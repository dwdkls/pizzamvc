using System;
using NHibernate;
using Pizza.Contracts;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Framework.ValueInjection;
using Pizza.Persistence;

namespace Pizza.Framework.Operations
{
    [Transactional]
    public class PersistenceModelsStore<TPersistenceModel, TEditModel, TCreateModel>
        where TPersistenceModel : IPersistenceModel, new()
        where TEditModel : IEditModelBase, new()
        where TCreateModel : ICreateModelBase, new()
    {
        protected readonly ISession session;

        public PersistenceModelsStore(ISession session)
        {
            this.session = session;
        }

        public virtual CrudOperationResult<int> Create(TCreateModel createModel, Action<TPersistenceModel> additionalOperations = null)
        {
            var persistenceModel = new TPersistenceModel();
            persistenceModel.InjectFromViewModel(createModel);

            if (additionalOperations != null)
            {
                additionalOperations(persistenceModel);
            }

            int id = (int)this.session.Save(persistenceModel);
            return new CrudOperationResult<int>(id);
        }

        public virtual CrudOperationResult Update(TEditModel editModel)
        {
            var persistenceModel = this.session.Get<TPersistenceModel>(editModel.Id);
            persistenceModel.InjectFromViewModel(editModel);
            this.session.Update(persistenceModel);
            return CrudOperationResult.Success;
        }

        public virtual CrudOperationResult Delete(int id)
        {
            var persistenceModel = this.session.Load<TPersistenceModel>(id);
            this.session.Delete(persistenceModel);
            return CrudOperationResult.Success;
        }
    }
}
