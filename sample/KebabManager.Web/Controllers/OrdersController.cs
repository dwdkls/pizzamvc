using System.Collections.Generic;
using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Framework.Operations.Requests.Configuration;
using Pizza.Mvc.Controllers;
using Pizza.Mvc.Grid.Metamodel;

namespace KebabManager.Web.Controllers
{
    public class OrdersController
        : GridControllerBase<IOrdersService, OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
        public OrdersController(IOrdersService service)
            : base(service)
        {
        }

        protected override Dictionary<ViewType, string> ViewNames
        {
            get
            {
                return new Dictionary<ViewType, string> {
                    { ViewType.Index, "Orders list" },
                    { ViewType.Create, "New Order form" },
                    { ViewType.Edit, "Edit Order form" },
                    { ViewType.Details, "Order details" },
                };
            }
        }

        protected override GridMetamodel<OrderGridModel> GetGridMetamodel()
        {
            var gridMetaModel = new GridMetamodelBuilder<OrderGridModel>()
                .SetCaption("Customers list")
                .AllowNew("New Customer").AllowEdit().AllowDelete().AllowDetails()
                .AddColumn(x => x.OrderDate, 200)
                .AddColumn(x => x.CustomerFirstName, 200)
                .AddDefaultSortColumn(x => x.CustomerLastName, SortMode.Descending, 150, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddColumn(x => x.ItemsCount, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .AddColumn(x => x.PaymentInfoNumber, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .Build();

            return gridMetaModel;
        }
    }
}