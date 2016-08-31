using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Customers;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Pizza.Mvc.GridConfig;

namespace KebabManager.Web.Controllers
{
    [Authorize]
    public class CustomersController
        : CrudControllerBase<ICustomersService, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
        public CustomersController(ICustomersService service) : base(service)
        {
        }

        //protected override Dictionary<ViewType, string> ViewNames
        //{
        //    get
        //    {
        //        return new Dictionary<ViewType, string> {
        //            { ViewType.Index, "Customers list" },
        //            { ViewType.Create, "New Customer form" },
        //            { ViewType.Edit, "Edit Customer form" },
        //            { ViewType.Details, "Customer details" },
        //        };
        //    }
        //}

        protected override GridMetamodel<CustomerGridModel> GetGridMetamodel()
        {
            var gridMetaModel = new GridMetamodelBuilder<CustomerGridModel>()
                //.SetCaption("Customers list")
                //.AllowNew("Create new Customer").AllowEdit("Go to edit").AllowDelete("Delete!").AllowDetails("Go to details")
                .AllowNew().AllowEdit().AllowDelete().AllowDetails()
                .AddDataColumn(x => x.LastName, 200)
                .AddDataColumn(x => x.FirstName, 200)
                .AddDefaultSortColumn(x => x.FingersCount, SortMode.Descending, 80, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddActionColumn<CustomerOrdersController>(x => x.Index(), "Orders", 80)
                .AddDataColumn(x => x.PreviousSurgeryDate, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .AddDataColumn(x => x.Animal, 100)
                .AddDataColumn(x => x.Type, 100)
                .Build();

            return gridMetaModel;
        }
    }
}