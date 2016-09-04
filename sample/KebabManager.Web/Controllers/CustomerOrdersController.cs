using System.Linq;
using System.Web.Mvc;
using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Orders;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Contracts.Operations.Results;
using Pizza.Mvc.Controllers;
using Pizza.Mvc.GridConfig;

namespace KebabManager.Web.Controllers
{
    [Authorize]
    public class CustomerOrdersController
        : CrudControllerBase<IOrdersService, OrderGridModel, OrderDetailsModel, OrderEditModel, OrderCreateModel>
    {
        public CustomerOrdersController(IOrdersService service) : base(service)
        {
        }

        protected override GridMetamodel<OrderGridModel> GetGridMetamodel()
        {
            var gridMetaModel = new GridMetamodelBuilder<OrderGridModel>()
                .AllowNew("Create new Order").AllowEdit("Go to edit").AllowDelete("Delete!").AllowDetails("Go to details")
                .AddDataColumn(x => x.OrderDate, 200)
                .AddDataColumn(x => x.CustomerFirstName, 200, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddDefaultSortColumn(x => x.CustomerLastName, SortMode.Descending, 150, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddDataColumn(x => x.ItemsCount, 150, ColumnWidthMode.Fixed)
                .AddDataColumn(x => x.PaymentInfoNumber, 150, ColumnWidthMode.Fixed)
                .Build();

            return gridMetaModel;
        }

        protected override DataPageResult<OrderGridModel> GetGridDataFromService(DataRequest<OrderGridModel> request)
        {
            // TODO: to some helper method? This won't work when we come back from details view  Think about better solution.
            var id = this.Request.UrlReferrer.Segments.Last();
            var customerId = int.Parse(id);

            return this.service.GetDataPageByCustomerId(request, customerId);
        }
    }
}