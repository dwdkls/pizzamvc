using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pizza.Framework.Operations.InternalUtils.RuntimeMetadata.Types
{
    public class ViewModelToPersistenceModelPropertyNamesMaps
    {
        public ReadOnlyDictionary<string, string> AllProperties { get; private set; }

        public ViewModelToPersistenceModelPropertyNamesMaps(Dictionary<string, string> allProperties)
        {
            this.AllProperties = new ReadOnlyDictionary<string, string>(allProperties); ;
        }
    }
}