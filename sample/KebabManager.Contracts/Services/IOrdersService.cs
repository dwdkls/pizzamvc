using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Contracts.Presentation.Operations;
using Pizza.Framework.Operations;

namespace KebabManager.Contracts.Services
{
    public interface IOrdersService
        : IGridServiceBase<OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
    }
}