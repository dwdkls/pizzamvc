using System;
using System.Collections.Generic;

namespace Pizza.Utils
{
    public class SimpleCache
    {
        private readonly Dictionary<Type, object> cachedFuncs = new Dictionary<Type, object>();

        public TCached GetFromCache<TCached>(Type keyType, Func<TCached> cachedFunc)
            where TCached : class
        {
            if (!this.cachedFuncs.ContainsKey(keyType))
            {
                this.cachedFuncs.Add(keyType, cachedFunc.Invoke());
            }
            
            return this.cachedFuncs[keyType] as TCached;
        }
    }

    public class SimpleCache<TCached>
         where TCached : class
    {
        private readonly Dictionary<Type, TCached> cachedFuncs = new Dictionary<Type, TCached>();

        public TCached GetFromCache(Type keyType, Func<TCached> cachedFunc)
        {
            if (!this.cachedFuncs.ContainsKey(keyType))
            {
                this.cachedFuncs.Add(keyType, cachedFunc.Invoke());
            }

            return this.cachedFuncs[keyType];
        }
    }
}