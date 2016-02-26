using Pizza.Contracts.Presentation.Operations;
using Pizza.Framework.Operations;
using Pizza.Framework.TestTypes.ViewModels.Customers;

namespace Pizza.Framework.IntegrationTests.TestServices
{
    public interface ICustomersGridService : IGridServiceBase<CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
    }
}