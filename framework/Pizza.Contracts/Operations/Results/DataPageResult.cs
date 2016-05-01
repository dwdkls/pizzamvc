using System.Collections.Generic;
using System.Text;

namespace Pizza.Contracts.Operations.Results
{
    public sealed class DataPageResult<T> : CrudOperationResultBase
    {
        public IList<T> Items { get; private set; }
        public PagingInfo PagingInfo { get; private set; }

        public DataPageResult(IList<T> items, int pageNumber, int pageSize, int totalItemsCount)
            : base(CrudOperationState.Success, null)
        {
            this.Items = items;
            this.PagingInfo = new PagingInfo(pageNumber, pageSize, totalItemsCount);
        }

        public DataPageResult(CrudOperationState state, string errorMessage) 
            : base(state, errorMessage)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("DataPage");
            sb.AppendLine("PagingInfo: " + this.PagingInfo);
            sb.AppendLine("Items:");
            foreach (var item in this.Items)
            {
                sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }
    }
}
