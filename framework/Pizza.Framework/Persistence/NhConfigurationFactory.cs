using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
using NHibernate.Event;
using Pizza.Framework.Persistence.Audit;
using Pizza.Framework.Persistence.Config;
using Pizza.Framework.Persistence.SoftDelete;
using System;
using System.Reflection;

namespace Pizza.Framework.Persistence
{
    public static class NhConfigurationFactory
    {
        public static Configuration BuildConfiguration(string connectionString, Assembly assembly)
        {
            var autoPersistenceModel = AutoMap.Assembly(assembly, new AutomappingConfiguration());
            return BuildConfiguration(connectionString, autoPersistenceModel);
        }

        public static Configuration BuildConfiguration(string connectionString, ITypeSource typeSource)
        {
            var autoPersistenceModel = AutoMap.Source(typeSource, new AutomappingConfiguration());
            return BuildConfiguration(connectionString, autoPersistenceModel);
        }

        private static Configuration BuildConfiguration(string connectionString, AutoPersistenceModel autoPersistenceModel)
        {
            Action<Configuration> config = c =>
            {
                c.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { new AuditingEventListener() };
                c.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { new AuditingEventListener() };
                c.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[] { new SoftDeleteEventListener() };
            };

            IPersistenceConfigurer sqlConfiguration = MsSqlConfiguration.MsSql2012
                .ConnectionString(connectionString)
                .ShowSql();

            var fullModel = autoPersistenceModel
                .Conventions.AddFromAssemblyOf<AutomappingConfiguration>()
                .AddFilter<SoftDeletableFilter>();

            var configuration = Fluently.Configure()
                .Database(sqlConfiguration)
                .Mappings(m => m.AutoMappings.Add(fullModel))
                .ExposeConfiguration(config)
                .BuildConfiguration();

            return configuration;
        }
    }
}