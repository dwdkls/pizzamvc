using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Diagnostics;
using NHibernate.Cfg;
using NHibernate.Event;
using NSubstitute;
using Pizza.Framework.Persistence.Audit;
using Pizza.Framework.Persistence.Config;
using Pizza.Framework.Persistence.SoftDelete;
using Pizza.Framework.Security;
using Pizza.Framework.TestTypes.Domain.PersistenceModels;

namespace Pizza.Framework.IntegrationTests.Base.Config
{
    public class NhConfigurationProvider
    {
        private class TestedPeristenceModelsSource : ITypeSource
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

        // TODO: move to some infrastructural helper or sth
        public static Configuration BuildNHibernateConfiguration(string connectionString)
        {
            IPizzaPrincipal pizzaPrincipal = Substitute.For<IPizzaPrincipal>();
            pizzaPrincipal.Id.Returns(997);

            IUserContext userContext = Substitute.For<IUserContext>();
            userContext.User.Returns(pizzaPrincipal);

            var auditor = new PersistenceModelAuditor(userContext);

            Action<Configuration> config = c =>
            {
                c.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new AuditingEventListener(auditor) };
                c.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new AuditingEventListener(auditor) };
                c.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[] { new SoftDeleteEventListener() };
            };

            var configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connectionString)
                    .ShowSql()
                )
                .Mappings(m => m.AutoMappings
                    .Add(
                        AutoMap.Source(new TestedPeristenceModelsSource(), new AutomappingConfiguration())
                            .Conventions.AddFromAssemblyOf<AutomappingConfiguration>()
                            .AddFilter<SoftDeletableFilter>()
                    )
                )
                .ExposeConfiguration(config)
                .BuildConfiguration();

            return configuration;
        }
    }
}