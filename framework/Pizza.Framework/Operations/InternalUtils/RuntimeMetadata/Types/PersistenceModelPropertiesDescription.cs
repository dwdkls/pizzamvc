using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types
{
    /// <summary>
    /// This class contains properties of some persistence model grouped by type important from NHibernate point of view,
    /// i.e.: other models, components and simple properties.
    /// </summary>
    public class PersistenceModelPropertiesDescription
    {
        public ReadOnlyCollection<PropInfo> JoinedModels { get; }
        public ReadOnlyCollection<PropInfo> ComponentProperties { get; }
        public ReadOnlyCollection<PropInfo> SimpleProperties { get; }

        public PersistenceModelPropertiesDescription(IList<PropInfo> simpleProperties, IList<PropInfo> componentProperties, IList<PropInfo> joinedModels)
        {
            this.JoinedModels = new ReadOnlyCollection<PropInfo>(joinedModels);
            this.ComponentProperties = new ReadOnlyCollection<PropInfo>(componentProperties);
            this.SimpleProperties = new ReadOnlyCollection<PropInfo>(simpleProperties);
        }
    }
}