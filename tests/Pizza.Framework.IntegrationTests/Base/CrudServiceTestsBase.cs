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
    // But in order to do that many things have to be configurable: connection string, persistence models source (as ITypeSource and Assembly)
    // and also application services 
    public abstract class CrudServiceTestsBase<TService>
    {
        public Configuration NhConfiguration
        {
            get { return this.Container.Resolve<Configuration>(); }
        }

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
            this.testContainerProvider = new TestContainerProvider(new TestedPeristenceModelsSource());
            this.nhSessionHelper = new NhSessionHelper(this.NhConfiguration);
            this.fixture = FixtureFactory.Build();

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
            var exporter = new SchemaExport(this.NhConfiguration);
            exporter.Drop(false, true);
            exporter.Create(false, true);
        }

        protected virtual void PrepareFixture() { }
        protected virtual void PrepareTest() { }
    }
}