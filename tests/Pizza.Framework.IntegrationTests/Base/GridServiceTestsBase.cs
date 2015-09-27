using Autofac;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.Base.Config;
using Pizza.Framework.IntegrationTests.Base.Helpers;
using Ploeh.AutoFixture;

namespace Pizza.Framework.IntegrationTests.Base
{
    // TODO: This could be class in framework, framework users maybe will use it for they own integration tests
    public abstract class GridServiceTestsBase<TService>
    {
        //TODO: allow user to replace this
        private const string connectionString = @"Data Source=.;Database=Pizza_Tests;Integrated Security=True;";

        private Configuration nhConfiguration;

        protected IFixture fixture;
        protected NhSessionHelper nhSessionHelper;
        protected ContainerProvider containerProvider;

        protected IContainer Container
        {
            get { return this.containerProvider.Container; }
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            LogManagerConfigurator.ConfigureLogManager();
            this.fixture = FixtureFactory.Build();

            this.nhSessionHelper = new NhSessionHelper(connectionString);
            this.nhConfiguration = NhConfigurationProvider.BuildNHibernateConfiguration(connectionString);
            this.containerProvider = new ContainerProvider(this.nhConfiguration);

            this.PrepareFixture();
        }

        [SetUp]
        public void TestSetup()
        {
            this.PrepareTest();
        }

        public TService GetService()
        {
            this.containerProvider.RefreshNhSessionRegistrationForNewScope();
            return this.Container.Resolve<TService>();
        }

        protected void RecreateDatabase()
        {
            var exporter = new SchemaExport(this.nhConfiguration);
            exporter.Drop(false, true);
            exporter.Create(false, true);
        }

        protected virtual void PrepareFixture() { }
        protected virtual void PrepareTest() { }
    }
}