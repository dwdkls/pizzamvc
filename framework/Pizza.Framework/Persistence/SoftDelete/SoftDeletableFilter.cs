using FluentNHibernate.Mapping;
using Pizza.Persistence;
using Pizza.Utils;

namespace Pizza.Framework.Persistence.SoftDelete
{
    public class SoftDeletableFilter : FilterDefinition
    {
        public const string FilterName = "SoftDeletable";
        private readonly string filterPropertyName = ObjectHelper.GetPropertyName<ISoftDeletable>(x => x.IsDeleted);

        public SoftDeletableFilter()
        {
            this.WithName(FilterName).WithCondition(string.Format("{0} = 0", this.filterPropertyName));
        }
    }
}