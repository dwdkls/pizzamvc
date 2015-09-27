using System;
using Pizza.Contracts.Presentation;
using Pizza.Framework.Operations.Requests.Configuration;

namespace Pizza.Framework.Operations.Requests
{
    public sealed class DataRequest<TGridModel>
        where TGridModel : IGridModelBase
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public SortConfiguration SortConfiguration { get; private set; }
        public FilterConfiguration<TGridModel> FilterConfiguration { get; private set; }

        public DataRequest(int currentPageNumber, int pageSize, 
            SortConfiguration sortConfiguration, FilterConfiguration<TGridModel> filterConfiguration)
        {
            if (sortConfiguration == null)
            {
                throw new ArgumentException("sortConfiguration");
            }
            if (filterConfiguration == null)
            {
                throw new ArgumentNullException("filterConfiguration");
            }

            this.PageNumber = currentPageNumber;
            this.PageSize = pageSize;
            this.FilterConfiguration = filterConfiguration;
            this.SortConfiguration = sortConfiguration;
        }
    }
}