using System;
using System.Linq;
using System.Reflection;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;
using Pizza.Persistence;
using Pizza.Persistence.Attributes;
using Pizza.Utils;

namespace Pizza.Framework.Operations.InternalUtils.RuntimeMetadata
{
    public class PersistenceModelPropertiesDescriptionGenerator
    {
        private static readonly Type persistenceModelBaseType = typeof(IPersistenceModel);

        public static PersistenceModelPropertiesDescription GenerateDescription(Type persistenceModelType)
        {
            var allProperties = persistenceModelType.GetProperties();

            var joinedEntities = allProperties.Where(IsJoinedPersistenceModel).Select(PropInfo.FromPropertyInfo).ToList();
            var componentEntities = allProperties.Where(IsComponent).Select(PropInfo.FromPropertyInfo).ToList();
            var simpleProperties = allProperties.Select(PropInfo.FromPropertyInfo).Except(joinedEntities).Except(componentEntities).ToList();

            return new PersistenceModelPropertiesDescription(simpleProperties, componentEntities, joinedEntities);
        }

        public static bool IsJoinedPersistenceModel(PropertyInfo propertyInfo)
        {
            return persistenceModelBaseType.IsAssignableFrom(propertyInfo.PropertyType);
        }

        public static bool IsComponent(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.GetAttribute<ComponentAttribute>() != null;
        }
    }
}