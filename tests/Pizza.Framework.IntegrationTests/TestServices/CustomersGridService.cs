using NHibernate;
using Pizza.Framework.Operations;
using Pizza.Framework.Persistence.Transactions;
using Pizza.Framework.TestTypes.Domain.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Customers;

namespace Pizza.Framework.IntegrationTests.TestServices
{
    [Transactional]
    public class CustomersGridService :
        GridServiceBase<Customer, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>,
        ICustomersGridService
    {
        public CustomersGridService(ISession session) : base(session)
        {
        }
    }
}