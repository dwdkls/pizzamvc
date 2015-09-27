using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy2;
using NHibernate;
using NHibernate.Cfg;
using Pizza.Framework.IntegrationTests.TestServices;
using Pizza.Framework.Persistence.SoftDelete;
using Pizza.Framework.Persistence.Transactions;

namespace Pizza.Framework.IntegrationTests.Base.Config
{
    public class ContainerProvider
    {
        public IContainer Container { get; private set; }

        public ContainerProvider(Configuration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TransactionManagingInterceptor>().AsSelf();

            var sessionFactory = configuration.BuildSessionFactory();

            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsInNamespaceOf<ICustomersGridService>())
                .AsImplementedInterfaces()
                .EnableClassInterceptors()
                .InterceptedBy(typeof(TransactionManagingInterceptor));

            this.Container = builder.Build();
        }

        public void RefreshNhSessionRegistrationForNewScope()
        {
            var builder = new ContainerBuilder();
            builder.Register(c =>
            {
                var session = this.Container.Resolve<ISessionFactory>().OpenSession();
                session.EnableFilter(SoftDeletableFilter.FilterName);

                return session;
            }).As<ISession>().InstancePerLifetimeScope();

            builder.Update(this.Container);
        }
    }
}