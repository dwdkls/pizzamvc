using System;
using NHibernate;
using Pizza.Contracts;
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

        public virtual int Create(TCreateModel createModel, Action<TPersistenceModel> additionalOperations = null)
        {
            var persistenceModel = new TPersistenceModel();
            persistenceModel.InjectFromViewModel(createModel);

            if (additionalOperations != null)
            {
                additionalOperations(persistenceModel);
            }

            return (int)this.session.Save(persistenceModel);
        }

        public virtual void Update(TEditModel editModel)
        {
            var persistenceModel = this.session.Get<TPersistenceModel>(editModel.Id);
            persistenceModel.InjectFromViewModel(editModel);
            this.session.Update(persistenceModel);
        }

        public virtual void Delete(int id)
        {
            var persistenceModel = this.session.Load<TPersistenceModel>(id);
            this.session.Delete(persistenceModel);
        }
    }
}
