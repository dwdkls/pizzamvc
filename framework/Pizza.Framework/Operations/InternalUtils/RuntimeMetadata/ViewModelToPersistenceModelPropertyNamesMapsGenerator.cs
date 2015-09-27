using System;
using System.Collections.Generic;
using System.Linq;
using Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types;

namespace Pizza.Framework.Operations.InternalUtils.RuntimeMetadata
{
    public class ViewModelToPersistenceModelPropertyNamesMapsGenerator
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
                    map.Add(vmProp.Name, string.Format("{0}.{1}", match.Name, vmProp.Name.Replace(match.Name, string.Empty)));
                }
            }

            return map;
        }
    }
}