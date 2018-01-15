using System.Linq;
using NHibernate.Criterion;
using Pizza.Contracts;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Utils;

namespace Pizza.Framework.Operations.InternalUtils
{
    internal class ProjectionsGenerator
    {
        private static string idPropertyName = nameof(IGridModelBase.Id);

        public static ProjectionList GenerateProjectionsList(ViewModelToPersistenceModelPropertyNamesMaps viewModelToPersistenceModelMap)
        {
            var projectionsList = Projections.ProjectionList();
            projectionsList.Add(Projections.Id(), idPropertyName);
            foreach (var property in viewModelToPersistenceModelMap.AllProperties.Where(p => p.Key != idPropertyName))
            {
                projectionsList.Add(Projections.Property(property.Value), property.Key);
            }

            return projectionsList;
        }
    }
}