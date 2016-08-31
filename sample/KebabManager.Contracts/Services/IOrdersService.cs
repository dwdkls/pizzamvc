using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Contracts.Operations;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;

namespace KebabManager.Contracts.Services
{
    public interface IOrdersService
        : ICrudServiceBase<OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
        DataPageResult<OrderGridModel> GetDataPageByCustomerId(DataRequest<OrderGridModel> request, int customerId);
    }
}