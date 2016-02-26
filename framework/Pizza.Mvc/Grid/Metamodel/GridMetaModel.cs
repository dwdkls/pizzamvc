using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pizza.Contracts.Presentation.Operations.Requests.Configuration;

namespace Pizza.Mvc.Grid.Metamodel
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

        protected List<ColumnMetamodel> columns;

        public ReadOnlyCollection<ColumnMetamodel> Columns
        {
            get { return new ReadOnlyCollection<ColumnMetamodel>(this.columns); }
        }

        public ReadOnlyCollection<string> ColumnNames
        {
            get { return new ReadOnlyCollection<string>(this.columns.Select(c => c.PropertyName).ToList()); }
        }
    }

    public class GridMetamodel<TGridModel> : GridMetamodel
    {
        public SortConfiguration DefaultSortSettings { get; protected set; }

        public GridMetamodel(string caption, string getGridDataActionName,
            LinkMetamodel newItemLink, LinkMetamodel detailsLink, LinkMetamodel editLink, LinkMetamodel deleteLink,
            List<ColumnMetamodel> columns, SortConfiguration defaultSortSettings)
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
