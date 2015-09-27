using System;
using System.Linq.Expressions;
using Pizza.Contracts.Presentation;

namespace Pizza.Framework.IntegrationTests.Base.Helpers
{
    public sealed class FilterData<TGridModel>
        where TGridModel : IGridModelBase
    {
        public Expression<Func<TGridModel, object>> Property { get; private set; }
        public string Value { get; private set; }

        public FilterData(Expression<Func<TGridModel, object>> property, string value)
        {
            this.Property = property;
            this.Value = value;
        }
    }
}