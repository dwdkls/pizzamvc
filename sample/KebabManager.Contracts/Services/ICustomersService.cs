using KebabManager.Contracts.ViewModels.Customers;
using Pizza.Contracts.Operations;

namespace KebabManager.Contracts.Services
{
    public interface ICustomersService 
        : ICrudServiceBase<CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
    }
}