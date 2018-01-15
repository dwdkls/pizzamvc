using System;

namespace Pizza.Contracts.Operations.Results
{
    public sealed class PagingInfo
    {
        public int CurrentPageNumber { get; }
        public int PageSize { get; }
        public int TotalItemsCount { get; }

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
            return $"Current: {this.CurrentPageNumber}, Size: {this.PageSize}, Total items: {this.TotalItemsCount}";
        }
    }
}
