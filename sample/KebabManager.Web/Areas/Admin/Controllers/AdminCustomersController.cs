using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Customers;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.Controllers;
using Pizza.Mvc.Grid.Metamodel;

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
                .AddColumn(x => x.LastName, 200)
                .AddColumn(x => x.FirstName, 200)
                .AddDefaultSortColumn(x => x.FingersCount, SortMode.Descending, 150, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddColumn(x => x.PreviousSurgeryDate, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .AddColumn(x => x.Animal, 100)
                .AddColumn(x => x.Type, 100)
                .Build();

            return gridMetaModel;
        }
    }
}