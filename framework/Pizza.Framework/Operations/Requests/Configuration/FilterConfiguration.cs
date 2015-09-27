using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pizza.Contracts.Presentation;

namespace Pizza.Framework.Operations.Requests.Configuration
{
    public sealed class FilterConfiguration<TGridModel>
        where TGridModel : IGridModelBase
    {
        public ReadOnlyCollection<FilterCondition> Conditions { get; private set; }

        private FilterConfiguration()
        {
            this.Conditions = new ReadOnlyCollection<FilterCondition>(new List<FilterCondition>());
        }

        public FilterConfiguration(FilterCondition condition)
            : this(new[] { condition })
        {
        }

        public FilterConfiguration(IEnumerable<FilterCondition> conditions)
        {
            this.Conditions = new ReadOnlyCollection<FilterCondition>(conditions.ToList());
        }

        //public FilterConfiguration(params FilterCondition<TGridModel>[] conditions)
        //{
        //    this.Conditions = new ReadOnlyCollection<FilterCondition<TGridModel>>(conditions.ToList());
        //}

        public static FilterConfiguration<TGridModel> Empty
        {
            get { return new FilterConfiguration<TGridModel>(); }
        }
    }
}