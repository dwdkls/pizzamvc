using System;
using System.Linq;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.Base;
using Pizza.Framework.IntegrationTests.TestServices;
using Pizza.Framework.TestTypes.Model.Common;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using Ploeh.AutoFixture;

namespace Pizza.Framework.IntegrationTests.OrdersCrudServiceTests
{
    [TestFixture]
    internal class OrdersCrudServiceStandardCasesTests : CrudServiceTestsBase<IOrdersCrudService>
    {
        private Order order;

        protected override void PrepareTest()
        {
            this.order = this.fixture.Create<Order>();

            this.nhSessionHelper.SaveInNewSession(this.order.Customer);
            this.nhSessionHelper.SaveInNewSession(this.order);
        }

        [Test]
        public void GetEditModel__Success()
        {
            var result = this.GetService().GetEditModel(this.order.Id);
            var editModel = result.Data;

            editModel.PaymentInfoOrderedDate = this.order.PaymentInfo.OrderedDate;

            editModel.ShouldNot(Be.Null);
            editModel.Id.ShouldEqual(this.order.Id);
            editModel.Note.ShouldEqual(this.order.Note);
            editModel.CustomerFirstName.ShouldEqual(this.order.Customer.FirstName);
            editModel.CustomerLastName.ShouldEqual(this.order.Customer.LastName);
            editModel.PaymentInfoOrderedDate.ShouldEqual(this.order.PaymentInfo.OrderedDate);
            editModel.Type.ShouldEqual(this.order.Type);
            editModel.OrderDate.ShouldEqual(this.order.OrderDate);
        }

        [Test]
        public void GetViewModel__Success()
        {
            var result = this.GetService().GetDetailsModel(this.order.Id);
            var viewModel = result.Data;

            viewModel.ShouldNot(Be.Null);
            viewModel.Id.ShouldEqual(this.order.Id);
            viewModel.Note.ShouldEqual(this.order.Note);
            viewModel.CustomerFirstName.ShouldEqual(this.order.Customer.FirstName);
            viewModel.CustomerLastName.ShouldEqual(this.order.Customer.LastName);
            viewModel.PaymentInfoOrderedDate.ShouldEqual(this.order.PaymentInfo.OrderedDate);
            viewModel.Type.ShouldEqual(this.order.Type);
            viewModel.OrderDate.ShouldEqual(this.order.OrderDate);
            viewModel.PaymentInfoState.ShouldEqual(this.order.PaymentInfo.State);
        }

        [Test]
        public void Create__ValidEditModel__Success()
        {
            const string noteText = "created note";

            var customerId = this.order.Customer.Id;

            var createModel = new OrderCreateModel
            {
                Note = noteText,
                Type = OrderType.NonStandard,
                OrderDate = DateTime.Now.AddMonths(1),
                PaymentInfoOrderedDate = DateTime.Now,
                Duration = 123,
                CustomerId = customerId,
            };

            var crudResult = this.GetService().Create(createModel);
            int orderId = crudResult.Data;

            var orderFromDb = this.nhSessionHelper.GetInNewSession<Order>(orderId);

            orderFromDb.ShouldNot(Be.Null);
            orderFromDb.Id.ShouldEqual(orderId);
            orderFromDb.Version.ShouldNot(Be.Empty);
            orderFromDb.Note.ShouldEqual(noteText);
        }

        [Test]
        public void Update__ValidEditModel__Success()
        {
            const string noteText = "created note";

            var orderId = this.order.Id;

            var editModel = new OrderEditModel
            {
                Id = orderId,
                Note = noteText,
            };

            this.GetService().Update(editModel);

            var orderFromDb = this.nhSessionHelper.ReturnInNewSession(x => x.Get<Order>(orderId));

            orderFromDb.ShouldNot(Be.Null);
            orderFromDb.Id.ShouldEqual(orderId);
            orderFromDb.Version.ShouldNot(Be.Empty);
            orderFromDb.Note.ShouldEqual(noteText);
        }
    }

    [TestFixture]
    internal class OrdersCrudServiceDeletionTests : CrudServiceTestsBase<IOrdersCrudService>
    {
        protected override void PrepareTest()
        {
            this.RecreateDatabase();
        }

        [Test]
        public void HardDelete_Success()
        {
            var customer = this.fixture.Create<Customer>();
            this.nhSessionHelper.SaveInNewSession(customer);

            var testOrders = this.fixture.CreateMany<Order>(3).ToList();
            testOrders.SetFixedValueForAllItems(x => x.Customer, customer);
            this.nhSessionHelper.SaveManyInNewSession(testOrders);

            this.GetService().Delete(testOrders[1].Id);

            var savedOrders = this.nhSessionHelper.ListQueryOverInNewSession<Order>();
            savedOrders.Should(Be.Not.Null);
            savedOrders.Should(Have.Count.EqualTo(2));
            savedOrders.Select(x => x.Id).Should(Be.EquivalentTo(new[] { testOrders[0].Id, testOrders[2].Id }));
        }
    }
}