using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Customers;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.Controllers;
using Pizza.Mvc.GridConfig;

namespace KebabManager.Web.Areas.Admin.Controllers
{
    public class AdminCustomersController : CrudControllerBase<ICustomersService, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
        public AdminCustomersController(ICustomersService service) : base(service)
        {
        }

        protected override GridMetamodel<CustomerGridModel> GetGridMetamodel()
        {
            var gridMetaModel = new GridMetamodelBuilder<CustomerGridModel>()
                .AllowNew().AllowEdit().AllowDelete().AllowDetails()
                .AddDataColumn(x => x.LastName, 200)
                .AddDataColumn(x => x.FirstName, 200)
                .AddDefaultSortColumn(x => x.FingersCount, SortMode.Descending, 150, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddDataColumn(x => x.PreviousSurgeryDate, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .AddDataColumn(x => x.Animal, 100)
                .AddDataColumn(x => x.Type, 100)
                .Build();

            return gridMetaModel;
        }
    }
}