using System;
using System.Collections.Generic;
using System.Linq;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;

namespace Pizza.Framework.Operations.InternalUtils.RuntimeMetadata
{
    public static class ViewModelToPersistenceModelPropertyNamesMapsGenerator
    {
        public static ViewModelToPersistenceModelPropertyNamesMaps Generate(
            Type viewModelType,
            PersistenceModelPropertiesDescription persistenceModelDescription)
        {
            var viewModelProps = viewModelType.GetProperties().Select(PropInfo.FromPropertyInfo).ToList();

            var simplePropsMap = viewModelProps.Intersect(persistenceModelDescription.SimpleProperties).ToDictionary(x => x.Name, x => x.Name);
            var joinedMap = GetMapForSubproperty(viewModelProps, persistenceModelDescription.JoinedModels);
            var componentsMap = GetMapForSubproperty(viewModelProps, persistenceModelDescription.ComponentProperties);

            var allProps = simplePropsMap
                .Union(componentsMap)
                .Union(joinedMap)
                .ToDictionary(x => x.Key, x => x.Value);

            var missingInPersistenceModel = viewModelProps.Where(x => allProps.All(v => v.Key != x.Name)).ToList();
            if (missingInPersistenceModel.Count > 0)
            {
                var propNames = missingInPersistenceModel.Select(x => x.Name).ToArray();
                var message = "Properties from view model not found in persistence model: " + string.Join(",", propNames);
                throw new ApplicationException(message);
            }

            return new ViewModelToPersistenceModelPropertyNamesMaps(allProps);
        }

        private static Dictionary<string, string> GetMapForSubproperty(List<PropInfo> viewModelProps, IList<PropInfo> propsToCompare)
        {
            var map = new Dictionary<string, string>();
            foreach (var vmProp in viewModelProps)
            {
                var match = propsToCompare.SingleOrDefault(x => vmProp.Name.StartsWith(x.Name));

                if (match != null)
                {
                    map.Add(vmProp.Name, $"{match.Name}.{vmProp.Name.Replace(match.Name, string.Empty)}");
                }
            }

            return map;
        }
    }
}