using Autofac;
using FluentNHibernate;
using NHibernate;
using NSubstitute;
using Pizza.Contracts.Security;
using Pizza.Framework.Persistence;
using Pizza.Framework.Persistence.SoftDelete;
using System.Reflection;

namespace Pizza.Framework.IntegrationTests.Base.Config
{
    public class TestContainerProvider
    {
        public IContainer Container { get; private set; }

        public TestContainerProvider(string connectionString, ITypeSource typeSource)
        {
            var builder = new ContainerBuilder()
                .RegisterPersistence(connectionString, typeSource)
                .RegisterApplicationServices(Assembly.GetExecutingAssembly());

            RegisterMockedUserContext(builder);

            this.Container = builder.Build();
        }

        private static void RegisterMockedUserContext(ContainerBuilder builder)
        {
            var pizzaPrincipal = Substitute.For<IPizzaPrincipal>();
            pizzaPrincipal.Id.Returns(997);

            var userContext = Substitute.For<IPizzaUserContext>();
            userContext.CurrentUser.Returns(pizzaPrincipal);

            builder.RegisterInstance(userContext).AsImplementedInterfaces();
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