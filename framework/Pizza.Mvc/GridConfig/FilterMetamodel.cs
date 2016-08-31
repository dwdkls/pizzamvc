using System.Collections.Generic;
using Pizza.Contracts.Operations.Requests.Configuration;

namespace Pizza.Mvc.GridConfig
{
    public class FilterMetamodel
    {
        public FilterOperator Operator { get; private set; }
        public Dictionary<string, string> SelectFilterMap { get; private set; }

        public FilterMetamodel(FilterOperator @operator, Dictionary<string, string> selectFilterMap)
        {
            this.Operator = @operator;
            this.SelectFilterMap = selectFilterMap;
        }
    }
}