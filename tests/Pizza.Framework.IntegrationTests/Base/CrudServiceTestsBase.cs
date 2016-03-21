using Autofac;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.Base.Config;
using Pizza.Framework.IntegrationTests.Base.Helpers;
using Pizza.Framework.Persistence;
using Ploeh.AutoFixture;

namespace Pizza.Framework.IntegrationTests.Base
{
    // TODO: This could be class in framework, framework users maybe will use it for they own integration tests
    public abstract class CrudServiceTestsBase<TService>
    {
        //TODO: allow user to replace this
        private const string connectionString = @"Data Source=.;Database=Pizza_Tests;Integrated Security=True;";

        private Configuration nhConfiguration;

        protected IFixture fixture;
        protected NhSessionHelper nhSessionHelper;
        protected TestContainerProvider testContainerProvider;

        protected IContainer Container
        {
            get { return this.testContainerProvider.Container; }
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            LogManagerConfigurator.ConfigureLogManager();
            this.fixture = FixtureFactory.Build();

            var peristenceModelsSource = new TestedPeristenceModelsSource();
            this.nhConfiguration = NhConfigurationFactory.BuildConfiguration(connectionString, peristenceModelsSource);
            this.nhSessionHelper = new NhSessionHelper(this.nhConfiguration);

            this.testContainerProvider = new TestContainerProvider(this.nhConfiguration);

            this.PrepareFixture();
        }

        [SetUp]
        public void TestSetup()
        {
            this.PrepareTest();
        }

        public TService GetService()
        {
            this.testContainerProvider.RefreshNhSessionRegistrationForNewScope();
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