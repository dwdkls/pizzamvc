using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using NHibernate;
using Pizza.Framework.Persistence;
using Pizza.Framework.Persistence.SoftDelete;
using Pizza.Framework.Persistence.Transactions;

namespace Pizza.Framework
{
    public class AutofacRegisterHelper
    {
        public static void RegisterPersistenceStuffAndServices(ContainerBuilder builder, 
            string connectionString, Assembly persistenceModelsAssembly, Assembly servicesAssembly)
        {
            var configuration = NhConfigurationFactory.BuildConfiguration(connectionString, persistenceModelsAssembly);

            var sessionFactory = configuration.BuildSessionFactory();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(c =>
            {
                var session = sessionFactory.OpenSession();
                session.EnableFilter(SoftDeletableFilter.FilterName);

                return session;
            }).As<ISession>().InstancePerLifetimeScope();

            builder.RegisterType<TransactionManagingInterceptor>().AsSelf();

            builder.RegisterAssemblyTypes(servicesAssembly)
                .Where(t => t.IsClass && t.Name.EndsWith("Service")) // TODO: don't use sufix...
                .AsImplementedInterfaces()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionManagingInterceptor));
        }
    }
}
