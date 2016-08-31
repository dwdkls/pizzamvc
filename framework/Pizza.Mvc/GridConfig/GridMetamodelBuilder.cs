using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.Controllers;
using Pizza.Mvc.GridConfig.Columns;
using Pizza.Mvc.GridConfig.Exceptions;
using Pizza.Mvc.Helpers;
using Pizza.Mvc.Resources;
using Pizza.Utils;

namespace Pizza.Mvc.GridConfig
{
    public class GridMetamodelBuilder<TGridModel>
    {
        private string gridCaption;
        private string getGridDataActionName = "GetGridData";
        private LinkMetamodel newItemLink;
        private LinkMetamodel detailsLink;
        private LinkMetamodel editLink;
        private LinkMetamodel deleteLink;
        private SortConfiguration defaultSortSettings;
        private readonly List<ColumnMetamodelBase> columns = new List<ColumnMetamodelBase>();

        public GridMetamodelBuilder()
        {
            this.newItemLink = LinkMetamodel.Disabled;
            this.editLink = LinkMetamodel.Disabled;
            this.detailsLink = LinkMetamodel.Disabled;
            this.deleteLink = LinkMetamodel.Disabled;
        }

        public GridMetamodelBuilder<TGridModel> SetCaption(string caption)
        {
            this.gridCaption = caption;
            return this;
        }

        public GridMetamodelBuilder<TGridModel> SetDataActionName(string gridDataActionName)
        {
            this.getGridDataActionName = gridDataActionName;
            return this;
        }

        public GridMetamodelBuilder<TGridModel> AllowNew(string text = null)
        {
            if (text == null)
            {
                text = UiTexts.GridButton_Create;
            }

            this.newItemLink = new LinkMetamodel(true, text);
            return this;
        }

        public GridMetamodelBuilder<TGridModel> AllowDetails(string text = null)
        {
            if (text == null)
            {
                text = UiTexts.GridButton_Details;
            }

            this.detailsLink = new LinkMetamodel(true, text);
            return this;
        }

        public GridMetamodelBuilder<TGridModel> AllowEdit(string text = null)
        {
            if (text == null)
            {
                text = UiTexts.GridButton_Edit;
            }

            this.editLink = new LinkMetamodel(true, text);
            return this;
        }

        public GridMetamodelBuilder<TGridModel> AllowDelete(string text = null)
        {
            if (text == null)
            {
                text = UiTexts.GridButton_Delete;
            }

            this.deleteLink = new LinkMetamodel(true, text);
            return this;
        }

        public GridMetamodelBuilder<TGridModel> AddDefaultSortColumn<TColumn>(
           Expression<Func<TGridModel, TColumn>> property, SortMode sortMode, int width = 150,
           ColumnWidthMode widthMode = ColumnWidthMode.Auto,
           FilterOperator filterOperator = FilterOperator.Auto)
        {
            this.defaultSortSettings = new SortConfiguration(ObjectHelper.GetPropertyName(property), sortMode);
            return this.AddDataColumn(property, width, widthMode, filterOperator);
        }

        public GridMetamodelBuilder<TGridModel> AddDataColumn<TColumn>(
            Expression<Func<TGridModel, TColumn>> property, int width = 150,
            ColumnWidthMode widthMode = ColumnWidthMode.Auto,
            FilterOperator filterOperator = FilterOperator.Auto)
        {
            var filterMetamodel = new FilterMetamodel(filterOperator, null);
            if (filterOperator == FilterOperator.Auto)
            {
                filterMetamodel = AutoResolveFilterConfigurationForColumn<TColumn>();
            }

            var column = PropertyColumnMetamodel.Create(property, width, widthMode, filterMetamodel);
            this.columns.Add(column);
            return this;
        }

        public GridMetamodelBuilder<TGridModel> AddActionColumn<TController>(
            Expression<Func<TController, ActionResult>> action, string caption, int width = 150,
            ColumnWidthMode widthMode = ColumnWidthMode.Auto)
            where TController : Controller
        {
            string controllerName = ControllerHelper.GetName<TController>();
            string actionName = ControllerHelper.GetActionName(action);

            var column = new ActionColumnMetamodel(controllerName, actionName, actionName, caption, width, widthMode);
            this.columns.Add(column);
            return this;
        }

        public GridMetamodel<TGridModel> Build()
        {
            if (string.IsNullOrEmpty(this.getGridDataActionName))
            {
                throw new GridBuildingException("GridDataActionName must be provided. Call SetDataActionName before Build.");
            }
            if (this.columns == null)
            {
                throw new GridBuildingException("Grid without any columns could not be created. Call AddColumn at least one time before Build.");
            }

            var gridMetaModel = new GridMetamodel<TGridModel>(this.gridCaption, this.getGridDataActionName,
                this.newItemLink, this.detailsLink, this.editLink, this.deleteLink, this.columns, this.defaultSortSettings);

            return gridMetaModel;
        }

        private static FilterMetamodel AutoResolveFilterConfigurationForColumn<TColumn>()
        {
            Dictionary<string, string> selectFilterMap = null;
            var filterOperator = FilterOperator.Auto;

            var columnType = typeof(TColumn).GetRealType();

            if (columnType == typeof(string))
            {
                filterOperator = FilterOperator.Like;
            }
            else if (columnType == typeof(DateTime))
            {
                filterOperator = FilterOperator.DateEquals;
            }
            else if (columnType.IsEnum)
            {
                selectFilterMap = EnumDisplayNameHelper.GetValueAndDescriptionMap(columnType);
                filterOperator = FilterOperator.Select;
            }
            else if (columnType.IsValueType)
            {
                // TODO: change to Equals
                filterOperator = FilterOperator.Disabled;
            }

            return new FilterMetamodel(filterOperator, selectFilterMap);
        }
    }
}