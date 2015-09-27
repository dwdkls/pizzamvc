using System.Collections.Generic;
using System.Text;

namespace Pizza.Framework.Operations.Results
{
    public sealed class DataPageResult<T>
    {
        public IList<T> Items { get; private set; }
        public PagingInfo PagingInfo { get; private set; }

        public DataPageResult(IList<T> items, int pageNumber, int pageSize, int totalItemsCount)
        {
            this.Items = items;
            this.PagingInfo = new PagingInfo(pageNumber, pageSize, totalItemsCount);
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
