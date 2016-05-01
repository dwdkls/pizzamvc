using NHibernate;
using NUnit.Framework;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.IntegrationTests.Base;
using Pizza.Framework.IntegrationTests.TestServices;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Customers;
using Pizza.Framework.ValueInjection;
using Ploeh.AutoFixture;
using System;
using System.Linq;
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

            var result = this.GetService().GetDetailsModel(testCustomer.Id);
            var viewModel = result.Data;

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

            var result = this.GetService().GetEditModel(testCustomer.Id);
            var editModel = result.Data;

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

            var crudResult = this.GetService().Create(createModel);

            crudResult.ShouldNot(Be.Null);
            crudResult.State.ShouldEqual(CrudOperationState.Success);
            crudResult.ErrorMessage.Should(Be.Null);

            var customerId = crudResult.Data;

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

            var result = this.GetService().GetEditModel(testCustomer.Id);
            var editModel = result.Data;
            editModel.Id = testCustomer.Id;
            editModel.Login = "NewLogin";
            editModel.FirstName = "John";
            editModel.LastName = "Smith";
            editModel.PreviousSurgeryDate = new DateTime(1999, 11, 19);

            var updateResult = this.GetService().Update(editModel);

            updateResult.Should(Be.Not.Null);
            updateResult.State.ShouldEqual(CrudOperationState.Success);
            updateResult.ErrorMessage.Should(Be.Null);


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

        [Test]
        public void Update_InSeparateSessions_CrudOperationStateOptimisticConcurrencyErrorReturned()
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
            var updateResult = this.GetService().Update(editModel2);
            updateResult.Should(Be.Not.Null);
            updateResult.State.ShouldEqual(CrudOperationState.OptimisticConcurrencyError);
            updateResult.ErrorMessage.Should(Be.Not.Null);


            // load and check if first update was succesfull
            var updatedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            updatedCustomer.FirstName.ShouldEqual("Andrew");
        }

        [Test]
        public void FailingUpdate_InSeparateSessionsWithMethodNotReturningCrudOperationResult_OptimisticConcurrencyExceptionThrown()
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

            Assert.DoesNotThrow(() => this.GetService().FailingUpdate(editModel1));
            Assert.Throws<StaleObjectStateException>(() => this.GetService().FailingUpdate(editModel2));

            // load and check if first update was succesfull
            var updatedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            updatedCustomer.FirstName.ShouldEqual("Andrew");
        }

        [Test]
        public void Update_InSeparateSessionsWithoutCrudService_OptimisticConcurrencyExceptionThrown()
        {
            var testCustomer = this.fixture.Create<Customer>();
            testCustomer.FirstName = "John";
            this.nhSessionHelper.SaveInNewSession(testCustomer);

            // simulate concurent updates
            var customer1 = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            customer1.FirstName = "Andrew";

            var customer2 = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            customer2.FirstName = "Paul";

            Assert.DoesNotThrow(() => this.nhSessionHelper.UpdateInNewSession(customer1));
            Assert.Throws<StaleObjectStateException>(() => this.nhSessionHelper.UpdateInNewSession(customer2));

            // load and check if first update was succesfull
            var updatedCustomer = this.nhSessionHelper.GetInNewSession<Customer>(testCustomer.Id);
            updatedCustomer.FirstName.ShouldEqual("Andrew");
        }
    }
}