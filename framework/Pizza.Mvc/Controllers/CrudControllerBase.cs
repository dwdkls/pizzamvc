using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcJqGrid;
using Pizza.Contracts;
using Pizza.Contracts.Operations;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;
using Pizza.Mvc.GridConfig;
using Pizza.Mvc.JqGrid;
using Pizza.Mvc.Resources;
using Pizza.Utils;

namespace Pizza.Mvc.Controllers
{
    public abstract class CrudControllerBase<TService, TGridModel, TViewModel, TEditModel, TCreateModel> : CoreControllerBase
        where TService : ICrudServiceBase<TGridModel, TViewModel, TEditModel, TCreateModel>
        where TGridModel : IGridModelBase
        where TViewModel : IDetailsModelBase
        where TEditModel : IEditModelBase
        where TCreateModel : ICreateModelBase
    {
        protected enum ViewType
        {
            Index,
            Details,
            Create,
            Edit,
        }

        private static readonly string idPropertyName = ObjectHelper.GetPropertyName<IGridModelBase>(x => x.Id);

        protected readonly TService service;
        protected static GridMetamodel<TGridModel> gridMetamodel;
        protected static Func<TGridModel, object> typeToJqGridObjectMapper;

        protected CrudControllerBase(TService service)
        {
            this.service = service;
            this.InitCachedConfiguration();
        }

        private static readonly Dictionary<CrudOperationState, string> errorMessagesMap = new Dictionary<CrudOperationState, string>() {
            { CrudOperationState.DatabaseError, Errors.DataBaseError },
            { CrudOperationState.OptimisticConcurrencyError, Errors.OptimisticConcurrencyError },
            { CrudOperationState.OtherError, Errors.OtherError },
        };

        private static HashSet<Type> configuredControllers = new HashSet<Type>();

        // TODO: [SMELL!!] this should be handled by IoC container not controller itself!
        private void InitCachedConfiguration()
        {
            var type = this.GetType();

            if (!configuredControllers.Contains(type))
            {
                gridMetamodel = this.GetGridMetamodel();
                // todo: remove this dependency!
                typeToJqGridObjectMapper = TypeToJqGridObjectMapperGenerator.GetMapper<TGridModel>(idPropertyName, gridMetamodel.ViewModelPropertiesNames);

                configuredControllers.Add(type);
            }
        }

        protected virtual Dictionary<ViewType, string> ViewNames
        {
            get
            {
                return new Dictionary<ViewType, string>
                {
                    {ViewType.Index, UiTexts.ViewTitle_Index},
                    {ViewType.Create, UiTexts.ViewTitle_Create},
                    {ViewType.Edit, UiTexts.ViewTitle_Edit},
                    {ViewType.Details, UiTexts.ViewTitle_Details},
                };
            }
        }

        protected abstract GridMetamodel<TGridModel> GetGridMetamodel();

        public ActionResult Index()
        {
            this.ViewBag.PageTitle = this.ViewNames[ViewType.Index];
            return this.View(gridMetamodel);
        }

        //public ActionResult Index(int id)
        //{
        //    this.ViewBag.PageTitle = this.ViewNames[ViewType.Index];
        //    return this.View(gridMetamodel);
        //}

        /// <summary>
        /// Method used by JqGrid to receive data from Controller.
        /// </summary>
        /// <param name="gridSettings">JqGrid settings.</param>
        /// <returns>JsonResult containing object compatible with JqGrid (includes paging information and records to show).</returns>
        public JsonResult GetGridData(GridSettings gridSettings)
        {
            // todo: remove this dependency!
            var request = MvcJqGridRequestBuilder.BuildGridDataRequest(gridSettings, gridMetamodel);

            var result = this.GetGridDataFromService(request);

            if (result.Succeed)
            {
                var items = result.Items.Select(typeToJqGridObjectMapper).ToList();
                var jsonData = new
                {
                    total = result.PagingInfo.TotalPages,
                    page = gridSettings.PageIndex,
                    records = result.PagingInfo.TotalItemsCount,
                    rows = items
                };

                return this.Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.HandleServiceErrorForJson(result);
            }
        }

        protected virtual DataPageResult<TGridModel> GetGridDataFromService(DataRequest<TGridModel> request)
        {
            return this.service.GetDataPage(request);
        }

        public ActionResult Details(int id)
        {
            var result = this.service.GetDetailsModel(id);
            var viewModel = result.Data;

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Details];
            return this.View(viewModel);
        }

        public virtual ActionResult Create()
        {
            var result = this.service.GetCreateModel();
            var createModel = result.Data;

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Create];
            return this.View(createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TCreateModel createModel)
        {
            if (this.ModelState.IsValid)
            {
                var result = this.service.Create(createModel);
                return this.HandleServiceResultForView(result, createModel);
            }

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Create];
            return this.View(createModel);
        }

        public ActionResult Edit(int id)
        {
            var result = this.service.GetEditModel(id);
            var editModel = result.Data;

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Edit];
            return this.View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TEditModel editModel)
        {
            if (this.ModelState.IsValid)
            {
                var result = this.service.Update(editModel);
                return this.HandleServiceResultForView(result, editModel);
            }

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Edit];
            return this.View(editModel);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var result = this.service.Delete(id);

            if (result.Succeed)
            {
                return this.Json("ok", JsonRequestBehavior.AllowGet);
            }

            return this.HandleServiceErrorForJson(result);
        }

        private ActionResult HandleServiceResultForView(CrudOperationResultBase result, IViewModelBase viewModel)
        {
            if (result.Succeed)
            {
                return this.RedirectToAction(GetActionName(x => this.Index()));
            }
            else
            {
                return this.HandleServiceErrorForAction(result, viewModel);
            }
        }

        private ActionResult HandleServiceErrorForAction(CrudOperationResultBase result, IViewModelBase viewModel)
        {
            var errorMessage = errorMessagesMap[result.State];
            this.ShowError(errorMessage);
            return this.View(viewModel);
        }

        private JsonResult HandleServiceErrorForJson(CrudOperationResultBase result)
        {
            this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errorMessage = errorMessagesMap[result.State];
            return this.Json(errorMessage, JsonRequestBehavior.AllowGet);
        }
    }
}