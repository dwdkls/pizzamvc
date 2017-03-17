using Pizza.Contracts.Operations;
using Pizza.Framework.TestTypes.ViewModels.Customers;

namespace Pizza.Framework.IntegrationTests.SutServices
{
    public interface ICustomersCrudService : ICrudServiceBase<CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
        void FailingUpdate(CustomerEditModel editModel);
    }
}