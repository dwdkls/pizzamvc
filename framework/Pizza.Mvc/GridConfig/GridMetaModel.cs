using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Mvc.GridConfig.Columns;

namespace Pizza.Mvc.GridConfig
{
    public enum ColumnWidthMode { Fixed, Auto }

    public abstract class GridMetamodel
    {
        public string Caption { get; protected set; }
        public string GetGridDataActionName { get; protected set; }

        public LinkMetamodel NewItemLink { get; protected set; }
        public LinkMetamodel DetailsLink { get; protected set; }
        public LinkMetamodel EditLink { get; protected set; }
        public LinkMetamodel DeleteLink { get; protected set; }

        protected List<ColumnMetamodelBase> columns;

        public ReadOnlyCollection<ColumnMetamodelBase> Columns
        {
            get { return new ReadOnlyCollection<ColumnMetamodelBase>(this.columns); }
        }

        public ReadOnlyCollection<string> ViewModelPropertiesNames
        {
            get
            {
                var propertyColumns = this.columns.OfType<PropertyColumnMetamodel>();
                var propertiesNames = propertyColumns.Select(c => c.Name).ToList();
                return new ReadOnlyCollection<string>(propertiesNames);
            }
        }
    }

    public class GridMetamodel<TGridModel> : GridMetamodel
    {
        public SortConfiguration DefaultSortSettings { get; protected set; }

        public GridMetamodel(string caption, string getGridDataActionName,
            LinkMetamodel newItemLink, LinkMetamodel detailsLink, LinkMetamodel editLink, LinkMetamodel deleteLink,
            List<ColumnMetamodelBase> columns, SortConfiguration defaultSortSettings)
        {
            this.Caption = caption;
            this.GetGridDataActionName = getGridDataActionName;
            this.NewItemLink = newItemLink;
            this.DetailsLink = detailsLink;
            this.EditLink = editLink;
            this.DeleteLink = deleteLink;
            this.columns = columns;
            this.DefaultSortSettings = defaultSortSettings;
        }
    }
}
