using System;
using System.Linq.Expressions;
using Pizza.Contracts;

namespace Pizza.Framework.IntegrationTests.Base.Helpers
{
    public sealed class FilterData<TGridModel>
        where TGridModel : IGridModelBase
    {
        public Expression<Func<TGridModel, object>> Property { get; }
        public string Value { get; }

        public FilterData(Expression<Func<TGridModel, object>> property, string value)
        {
            this.Property = property;
            this.Value = value;
        }
    }
}