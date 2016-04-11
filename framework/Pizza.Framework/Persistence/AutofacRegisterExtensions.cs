using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Event;
using Pizza.Framework.Persistence.Audit;
using Pizza.Framework.Persistence.Config;
using Pizza.Framework.Persistence.SoftDelete;
using Pizza.Framework.Persistence.Transactions;

namespace Pizza.Framework.Persistence
{
    public static class AutofacRegisterExtensions
    {
        public static ContainerBuilder RegisterApplicationServices(this ContainerBuilder builder, Assembly servicesAssembly)
        {
            var services = servicesAssembly.DefinedTypes
                .Where(t => t.IsClass && t.Name.EndsWith("Service"))
                .Cast<Type>()
                .ToArray();

            builder.RegisterTypes(services)
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionManagingInterceptor));

            builder.RegisterTypes(services)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionManagingInterceptor));

            return builder;
        }

        public static ContainerBuilder RegisterPersistence(this ContainerBuilder builder, string connectionString, Assembly persistenceModelsAssembly)
        {
            var autoPersistenceModel = AutoMap.Assembly(persistenceModelsAssembly, new AutomappingConfiguration());
            InternalRegisterPersistence(builder, connectionString, autoPersistenceModel);
            return builder;
        }

        public static ContainerBuilder RegisterPersistence(this ContainerBuilder builder, string connectionString, ITypeSource persistenceModelsTypeSource)
        {
            var autoPersistenceModel = AutoMap.Source(persistenceModelsTypeSource, new AutomappingConfiguration());
            InternalRegisterPersistence(builder, connectionString, autoPersistenceModel);
            return builder;
        }

        private static void InternalRegisterPersistence(ContainerBuilder builder, string connectionString, AutoPersistenceModel autoPersistenceModel)
        {
            builder.RegisterType<PersistenceModelAuditor>().AsSelf();
            builder.RegisterType<TransactionManagingInterceptor>().AsSelf();

            builder.Register(cx => RegisterConfiguration(cx, connectionString, autoPersistenceModel))
                .As<Configuration>().SingleInstance();

            builder.Register(cx =>
            {
                var configuration = cx.Resolve<Configuration>();
                var sessionFactory = configuration.BuildSessionFactory();
                return sessionFactory;
            }).As<ISessionFactory>().SingleInstance();

            builder.Register(cx =>
            {
                var sessionFactory = cx.Resolve<ISessionFactory>();
                var session = sessionFactory.OpenSession();
                session.EnableFilter(SoftDeletableFilter.FilterName);

                return session;
            }).As<ISession>().InstancePerLifetimeScope();
        }

        private static Configuration RegisterConfiguration(IComponentContext cx, string connectionString, AutoPersistenceModel autoPersistenceModel)
        {
            var persistenceModelAuditor = cx.Resolve<PersistenceModelAuditor>();
            var auditingEventListener = new AuditingEventListener(persistenceModelAuditor);

            Action<Configuration> config = c =>
            {
                c.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] {auditingEventListener};
                c.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] {auditingEventListener};
                c.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[] {new SoftDeleteEventListener()};
            };

            var sqlConfiguration = MsSqlConfiguration.MsSql2012
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
