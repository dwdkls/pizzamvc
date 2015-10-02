using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Orders;
using KebabManager.Model.PersistenceModels;
using NHibernate;
using Pizza.Framework.Operations;

namespace KebabManager.Application.Services
{
    public class OrdersGridService
        : GridServiceBase<Order, OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>, IOrdersService
    {
        public OrdersGridService(ISession session)
            : base(session)
        {
        }
    }
}