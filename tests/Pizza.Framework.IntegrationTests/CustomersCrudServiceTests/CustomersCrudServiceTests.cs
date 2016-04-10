using System;
using System.Linq;
using NHibernate;
using NUnit.Framework;
using Pizza.Framework.IntegrationTests.Base;
using Pizza.Framework.IntegrationTests.TestServices;
using Pizza.Framework.Persistence.Exceptions;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Customers;
using Pizza.Framework.ValueInjection;
using Ploeh.AutoFixture;
using StringGenerator = Pizza.Framework.DataGeneration.StringGenerator;

namespace Pizza.Framework.IntegrationTests.CustomersCrudServiceTests
{
    internal class CustomersCrudServiceTests : CrudServiceTestsBase<ICustomersCrudService>
    {
        protected override void PrepareTest()
        {
            this.RecreateDatabase();
        }

        [Test]
        public void GetViewModel_AllPropertiesHaveValidValues()
        {
            var testCustomer = this.fixture.Create<Customer>();
            this.nhSessionHelper.SaveInNewSession(testCustomer);

            var viewModel = this.GetService().GetDetailsModel(testCustomer.Id);

            viewModel.ShouldNot(Be.Null);
            viewModel.Id.ShouldEqual(testCustomer.Id);
            viewModel.Login.ShouldEqual(testCustomer.Login);
            viewModel.FirstName.ShouldEqual(testCustomer.FirstName);
            viewModel.LastName.ShouldEqual(testCustomer.LastName);
            viewModel.PreviousSurgeryDate.ShouldEqual(testCustomer.PreviousSurgeryDate);
        }

        [Test]
        public void GetEditModel_AllPropertiesHaveValidValues()
        {
            var testCustomer = this.fixture.Create<Customer>();
            this.nhSessionHelper.SaveInNewSession(testCustomer);

            var editModel = this.GetService().GetEditModel(testCustomer.Id);

            editModel.ShouldNot(Be.Null);
            editModel.Id.ShouldEqual(testCustomer.Id);
            editModel.Version.ShouldNot(Be.Null);
            editModel.Login.ShouldEqual(testCustomer.Login);
            editModel.FirstName.ShouldEqual(testCustomer.FirstName);
            editModel.LastName.ShouldEqual(testCustomer.LastName);
            editModel.PreviousSurgeryDate.ShouldEqual(testCustomer.PreviousSurgeryDate);
        }

        [Test]
        public void Create_ValidCreateModel_AllPropertiesHaveValidValues()
        {
            var createModel = this.fixture.Create<CustomerCreateModel>();
            createModel.Login = StringGenerator.GenerateRandomString(30);
            createModel.Password = StringGenerator.GenerateRandomString(128);

            var customerId = this.GetService().Create(createModel);

            var savedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(customerId);

            savedCustomer.ShouldNot(Be.Null);
            savedCustomer.Id.ShouldEqual(customerId);
            savedCustomer.Version.ShouldNot(Be.Empty);
            savedCustomer.Login.ShouldEqual(createModel.Login);
            savedCustomer.Password.ShouldEqual(createModel.Password);
            savedCustomer.FirstName.ShouldEqual(createModel.FirstName);
            savedCustomer.LastName.ShouldEqual(createModel.LastName);
            savedCustomer.PreviousSurgeryDate.ShouldEqual(createModel.PreviousSurgeryDate);
        }

        [Test]
        public void Update_ValidEditModel_AllPropertiesHaveValidValues()
        {
            var testCustomer = this.fixture.Create<Customer>();
            this.nhSessionHelper.SaveInNewSession(testCustomer);

            var editModel = this.GetService().GetEditModel(testCustomer.Id);
            editModel.Id = testCustomer.Id;
            editModel.Login = "NewLogin";
            editModel.FirstName = "John";
            editModel.LastName = "Smith";
            editModel.PreviousSurgeryDate = new DateTime(1999, 11, 19);

            this.GetService().Update(editModel);

            var savedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);

            savedCustomer.ShouldNot(Be.Null);
            savedCustomer.Id.ShouldEqual(testCustomer.Id);
            savedCustomer.Version.ShouldNot(Be.Empty);
            savedCustomer.Login.ShouldNotEqual(editModel.Login);
            savedCustomer.FirstName.ShouldEqual(editModel.FirstName);
            savedCustomer.LastName.ShouldEqual(editModel.LastName);
            savedCustomer.PreviousSurgeryDate.ShouldEqual(editModel.PreviousSurgeryDate);
        }

        [Test]
        public void SoftDelete_Success()
        {
            var testCustomers = this.fixture.CreateMany<Customer>(3).ToList();
            this.nhSessionHelper.SaveManyInNewSession(testCustomers);

            this.GetService().Delete(testCustomers[1].Id);

            var visibleCustomers = this.nhSessionHelper.ListQueryOverInNewSession<Customer>();
            visibleCustomers.Should(Be.Not.Null);
            visibleCustomers.Should(Have.Count.EqualTo(2));
            visibleCustomers.Select(x => x.Id).Should(Be.EquivalentTo(new[] { testCustomers[0].Id, testCustomers[2].Id }));

            var softDeletedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(testCustomers[1].Id);
            softDeletedCustomer.Should(Be.Not.Null);
        }

        private void Update123(ISession session, CustomerEditModel editModel)
        {
            var persistenceModel = session.Load<Customer>(editModel.Id);
            persistenceModel.InjectFromViewModel(editModel);
            session.Update(persistenceModel);
        }

        [Test]
        public void Update_InSeparateSessions_OptimisticConcurrencyExceptionThrown()
        {
            var testCustomer = this.fixture.Create<Customer>();
            testCustomer.FirstName = "John";
            this.nhSessionHelper.SaveInNewSession(testCustomer);

            // simulate concurent updates
            var customer1 = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            var editModel1 = Injector.CreateViewModelFromPersistenceModel<Customer, CustomerEditModel>(customer1);
            editModel1.FirstName = "Andrew";

            var customer2 = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            var editModel2 = Injector.CreateViewModelFromPersistenceModel<Customer, CustomerEditModel>(customer2);
            editModel2.FirstName = "Paul";

            Assert.DoesNotThrow(() => this.GetService().Update(editModel1));
            Assert.Throws<OptimisticConcurrencyException>(() => this.GetService().Update(editModel2));

            // load and check if first update was succesfull
            var updatedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            updatedCustomer.FirstName.ShouldEqual("Andrew");
        }
    }
}