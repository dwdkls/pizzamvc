using System.Collections.Generic;
using System.Web;
using Pizza.Framework.Operations.Requests.Configuration;
using Pizza.Mvc.Controllers;
using Pizza.Mvc.Grid.Metamodel;
using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Customers;
using System.Web.Mvc;

namespace KebabManager.Web.Controllers
{
    [Authorize]
    public class CustomersController
        : GridControllerBase<ICustomersService, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>
    {
        public CustomersController(ICustomersService service) : base(service)
        {
        }

        protected override Dictionary<ViewType, string> ViewNames
        {
            get
            {
                return new Dictionary<ViewType, string> {
                    { ViewType.Index, "Customers list" },
                    { ViewType.Create, "New Customer form" },
                    { ViewType.Edit, "Edit Customer form" },
                    { ViewType.Details, "Customer details" },
                };
            }
        }

        protected override GridMetamodel<CustomerGridModel> GetGridMetamodel()
        {
            var gridMetaModel = new GridMetamodelBuilder<CustomerGridModel>()
                .SetCaption("Customers list")
                .AllowNew("New Customer").AllowEdit().AllowDelete().AllowDetails()
                .AddColumn(x => x.LastName, 200)
                .AddColumn(x => x.FirstName, 200)
                .AddDefaultSortColumn(x => x.FingersCount, SortMode.Descending, 150, ColumnWidthMode.Fixed, FilterOperator.Disabled)
                .AddColumn(x => x.PreviousSurgeryDate, 150, ColumnWidthMode.Fixed, FilterOperator.DateEquals)
                .Build();

            return gridMetaModel;
        }
    }
}