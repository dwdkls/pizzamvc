using KebabManager.Contracts.ViewModels.Customers;
using Pizza.Framework.Operations;

namespace KebabManager.Contracts.Services
{
    public interface ICustomersService 
        : IGridServiceBase<CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
    }
}