using Pizza.Framework.Operations;
using Pizza.Framework.TestTypes.ViewModels.Orders;

namespace Pizza.Framework.IntegrationTests.TestServices
{
    public interface IOrdersGridService : IGridServiceBase<OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
    }
}