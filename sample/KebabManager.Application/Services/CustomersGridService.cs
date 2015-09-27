using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Customers;
using KebabManager.Model.PersistenceModels;
using NHibernate;
using Pizza.Framework.Operations;

namespace KebabManager.Application.Services
{
    public class CustomersGridService 
        : GridServiceBase<Customer, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>, ICustomersService
    {
        public CustomersGridService(ISession session) : base(session)
        {
        }
    }
}
