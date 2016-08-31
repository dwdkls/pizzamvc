using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Orders;
using KebabManager.Model.PersistenceModels;
using NHibernate;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.Operations;

namespace KebabManager.Application.Services
{
    public class OrdersCrudService
        : CrudServiceBase<Order, OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>, IOrdersService
    {
        public OrdersCrudService(ISession session)
            : base(session)
        {
        }

        public DataPageResult<OrderGridModel> GetDataPageByCustomerId(DataRequest<OrderGridModel> request, int customerId)
        {
            return this.GetDataPage(request, order => order.Customer.Id == customerId);
        }
    }
}