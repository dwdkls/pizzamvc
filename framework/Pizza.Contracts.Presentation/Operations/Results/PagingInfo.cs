using System;

namespace Pizza.Contracts.Presentation.Operations.Results
{
    public sealed class PagingInfo
    {
        public int CurrentPageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalItemsCount { get; private set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)this.TotalItemsCount / this.PageSize); }
        }

        public PagingInfo(int currentPageNumber, int pageSize, int totalItemsCount)
        {
            this.CurrentPageNumber = currentPageNumber;
            this.PageSize = pageSize;
            this.TotalItemsCount = totalItemsCount;
        }

        public override string ToString()
        {
            return string.Format("Current: {0}, Size: {1}, Total items: {2}",
                this.CurrentPageNumber, this.PageSize, this.TotalItemsCount);
        }
    }
}
