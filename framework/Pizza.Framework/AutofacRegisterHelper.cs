using System.Linq;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using NHibernate;
using NHibernate.Cfg;
using Pizza.Framework.Persistence.Audit;
using Pizza.Framework.Persistence.SoftDelete;
using Pizza.Framework.Persistence.Transactions;
using System.Reflection;

namespace Pizza.Framework
{
    public class AutofacRegisterHelper
    {
        public static void RegisterPersistenceStuffAndServices(ContainerBuilder builder,
            Configuration configuration, Assembly persistenceModelsAssembly, Assembly servicesAssembly)
        {
            builder.RegisterType<PersistenceModelAuditor>().AsSelf();
            builder.RegisterType<TransactionManagingInterceptor>().AsSelf();

            var sessionFactory = configuration.BuildSessionFactory();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(c =>
            {
                var session = sessionFactory.OpenSession();
                session.EnableFilter(SoftDeletableFilter.FilterName);

                return session;
            }).As<ISession>().InstancePerLifetimeScope();

            var services = servicesAssembly.DefinedTypes
                .Where(t => t.IsClass && t.Name.EndsWith("Service"))
                .ToArray();

            builder.RegisterTypes(services)
                .AsSelf()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionManagingInterceptor));

            builder.RegisterTypes(services)
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(TransactionManagingInterceptor));
        }
    }
}
