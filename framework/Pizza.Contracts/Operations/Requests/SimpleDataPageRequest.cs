namespace Pizza.Contracts.Operations.Requests
{
    public sealed class SimpleDataPageRequest
    {
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }

        public SimpleDataPageRequest(int currentPageNumber, int pageSize)
        {
            this.PageNumber = currentPageNumber;
            this.PageSize = pageSize;
        }
    }
}