namespace Pizza.Contracts.Operations.Requests
{
    public sealed class SimpleDataPageRequest
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public SimpleDataPageRequest(int currentPageNumber, int pageSize)
        {
            this.PageNumber = currentPageNumber;
            this.PageSize = pageSize;
        }
    }
}