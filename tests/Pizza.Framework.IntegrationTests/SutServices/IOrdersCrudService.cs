using Pizza.Contracts.Operations;
using Pizza.Framework.TestTypes.ViewModels.Orders;

namespace Pizza.Framework.IntegrationTests.SutServices
{
    public interface IOrdersCrudService : ICrudServiceBase<OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
    }
}