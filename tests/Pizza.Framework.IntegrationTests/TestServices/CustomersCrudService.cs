using NHibernate;
using Pizza.Framework.Operations;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Customers;
using Pizza.Framework.ValueInjection;

namespace Pizza.Framework.IntegrationTests.TestServices
{
    [Transactional]
    public class CustomersCrudService :
        CrudServiceBase<Customer, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>,
        ICustomersCrudService
    {
        public CustomersCrudService(ISession session) : base(session)
        {
        }

        public void FailingUpdate(CustomerEditModel editModel)
        {
            var persistenceModel = this.session.Get<Customer>(editModel.Id);
            persistenceModel.InjectFromViewModel(editModel);
            this.session.Update(persistenceModel);
        }
    }
}