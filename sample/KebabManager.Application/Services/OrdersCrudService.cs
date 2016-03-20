using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Orders;
using KebabManager.Model.PersistenceModels;
using NHibernate;
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
    }
}