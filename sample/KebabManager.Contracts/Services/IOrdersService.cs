using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Contracts.Operations;

namespace KebabManager.Contracts.Services
{
    public interface IOrdersService
        : IGridServiceBase<OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
    }
}