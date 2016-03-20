using Pizza.Contracts.Operations;
using Pizza.Framework.Operations;
using Pizza.Framework.TestTypes.ViewModels.Customers;

namespace Pizza.Framework.IntegrationTests.TestServices
{
    public interface ICustomersCrudService : ICrudServiceBase<CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
    }
}