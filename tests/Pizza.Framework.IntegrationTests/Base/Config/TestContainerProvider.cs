using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NSubstitute;
using Pizza.Framework.Persistence.SoftDelete;
using System.Reflection;
using Pizza.Contracts.Security;
using Pizza.Framework.TestTypes.Model.PersistenceModels;

namespace Pizza.Framework.IntegrationTests.Base.Config
{
    public class TestContainerProvider
    {
        public IContainer Container { get; private set; }

        public TestContainerProvider(Configuration configuration)
        {
            var builder = new ContainerBuilder();

            RegisterMockedUserContext(builder);
            AutofacRegisterHelper.RegisterPersistenceStuffAndServices(builder, configuration, typeof(Customer).Assembly, Assembly.GetExecutingAssembly());

            this.Container = builder.Build();
            PizzaServerContext.Initialize(this.Container);
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