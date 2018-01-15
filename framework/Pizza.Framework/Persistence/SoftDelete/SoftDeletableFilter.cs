using FluentNHibernate.Mapping;
using Pizza.Persistence;
using Pizza.Utils;

namespace Pizza.Framework.Persistence.SoftDelete
{
    public class SoftDeletableFilter : FilterDefinition
    {
        public const string FilterName = "SoftDeletable";
        private readonly string filterPropertyName = nameof(ISoftDeletable.IsDeleted);

        public SoftDeletableFilter()
        {
            this.WithName(FilterName).WithCondition($"{this.filterPropertyName} = 0");
        }
    }
}