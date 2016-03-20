using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Contracts.Operations;

namespace KebabManager.Contracts.Services
{
    public interface IOrdersService
        : ICrudServiceBase<OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
    }
}