using System;
using Pizza.Contracts.Operations.Requests.Configuration;

namespace Pizza.Contracts.Operations.Requests
{
    public sealed class DataRequest<TGridModel>
        where TGridModel : IGridModelBase
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public SortConfiguration SortConfiguration { get; }
        public FilterConfiguration<TGridModel> FilterConfiguration { get; }

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