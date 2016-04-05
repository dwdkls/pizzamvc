using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MvcJqGrid;
using Pizza.Contracts;
using Pizza.Contracts.Operations;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.Utils;
using Pizza.Mvc.Grid;
using Pizza.Mvc.Grid.Metamodel;
using Pizza.Mvc.Resources;

namespace Pizza.Mvc.Controllers
{
    public abstract class CrudControllerBase<TService, TGridModel, TViewModel, TEditModel, TCreateModel> : CoreControllerBase
        where TService : ICrudServiceBase<TGridModel, TViewModel, TEditModel, TCreateModel>
        where TGridModel : IGridModelBase
        where TViewModel : IDetailsModelBase
        where TEditModel : IEditModelBase
        where TCreateModel : ICreateModelBase
    {
        protected enum ViewType { Index, Details, Create, Edit, }

        private static readonly string idPropertyName = ObjectHelper.GetPropertyName<IGridModelBase>(x => x.Id);

        // TODO: to separate service?
        //protected static readonly CultureInfo currentCulture = new CultureInfo("en-US");

        protected readonly TService service;
        protected static GridMetamodel<TGridModel> gridMetamodel;
        protected static Func<TGridModel, object> typeToJqGridObjectMapper;

        protected CrudControllerBase(TService service)
        {
            this.service = service;
            this.InitCachedConfiguration();
        }


        private static HashSet<Type> configuredControllers = new HashSet<Type>();

        // TODO: [SMELL!!] this should be handled by IoC container not controller itself!
        private void InitCachedConfiguration()
        {
            var type = this.GetType();

            if (!configuredControllers.Contains(type))
            {
                gridMetamodel = this.GetGridMetamodel();
                typeToJqGridObjectMapper = TypeToJqGridObjectMapperGenerator.GetMapper<TGridModel>(idPropertyName, gridMetamodel.ColumnNames);

                configuredControllers.Add(type);
            }
        }

        protected virtual Dictionary<ViewType, string> ViewNames
        {
            get
            {
                return new Dictionary<ViewType, string> {
                    { ViewType.Index, UiTexts.ViewTitle_Index },
                    { ViewType.Create, UiTexts.ViewTitle_Create },
                    { ViewType.Edit, UiTexts.ViewTitle_Edit },
                    { ViewType.Details, UiTexts.ViewTitle_Details },
                };
            }
        }

        protected abstract GridMetamodel<TGridModel> GetGridMetamodel();

        public ActionResult Index()
        {
            this.ViewBag.PageTitle = this.ViewNames[ViewType.Index];
            return this.View(gridMetamodel);
        }

        /// <summary>
        /// Method used by JqGrid to receive data from Controller.
        /// </summary>
        /// <param name="gridSettings">JqGrid settings.</param>
        /// <returns>JsonResult containing object compatible with JqGrid (includes paging information and records to show).</returns>
        public JsonResult GetGridData(GridSettings gridSettings)
        {
            var request = MvcJqGridRequestBuilder.BuildGridDataRequest(gridSettings, gridMetamodel);

            var listResult = this.GetGridDataFromService(request);

            var items = listResult.Items.Select(typeToJqGridObjectMapper).ToList();
            var jsonData = new
            {
                total = listResult.PagingInfo.TotalPages,
                page = gridSettings.PageIndex,
                records = listResult.PagingInfo.TotalItemsCount,
                rows = items
            };

            return this.Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        protected virtual DataPageResult<TGridModel> GetGridDataFromService(DataRequest<TGridModel> request)
        {
            return this.service.GetDataPage(request);
        }

        public ActionResult Details(int id)
        {
            var viewModel = this.service.GetDetailsModel(id);

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Details];
            return this.View(viewModel);
        }

        public virtual ActionResult Create()
        {
            var createModel = this.service.GetCreateModel();

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Create];
            return this.View(createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Create(TCreateModel createModel)
        {
            if (this.ModelState.IsValid)
            {
                this.service.Create(createModel);
                return this.RedirectToAction(GetActionName(x => this.Index()));
            }

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Create];
            return this.View(createModel);
        }

        public ActionResult Edit(int id)
        {
            var editModel = this.service.GetEditModel(id);

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Edit];
            return this.View(editModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TEditModel editModel)
        {
            if (this.ModelState.IsValid)
            {
                this.service.Update(editModel);
                return this.RedirectToAction(GetActionName(x => this.Index()));
            }

            this.ViewBag.PageTitle = this.ViewNames[ViewType.Edit];
            return this.View(editModel);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            this.service.Delete(id);
            return this.Json("ok", JsonRequestBehavior.AllowGet);
        }
    }
}