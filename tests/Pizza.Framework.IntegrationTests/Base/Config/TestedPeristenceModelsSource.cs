using FluentNHibernate;
using FluentNHibernate.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using Pizza.Framework.TestTypes.Model.PersistenceModels;

namespace Pizza.Framework.IntegrationTests.Base.Config
{
    class TestedPeristenceModelsSource : ITypeSource
    {
        public IEnumerable<Type> GetTypes()
        {
            var orderType = typeof(Order);
            var types = orderType.Assembly.GetExportedTypes().Where(t => t.Namespace == orderType.Namespace);

            return types;
        }

        public void LogSource(IDiagnosticLogger logger)
        {
        }

        public string GetIdentifier()
        {
            return "TestTypeSource";
        }
    }
}