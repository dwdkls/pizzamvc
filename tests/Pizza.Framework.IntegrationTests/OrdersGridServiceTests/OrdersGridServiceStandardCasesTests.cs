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

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests
{
    [TestFixture]
    internal class OrdersGridServiceStandardCasesTests : GridServiceTestsBase<IOrdersCrudService>
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
            var editModel = this.GetService().GetEditModel(order.Id);

            editModel.PaymentInfoOrderedDate = order.PaymentInfo.OrderedDate;

            editModel.ShouldNot(Be.Null);
            editModel.Id.ShouldEqual(order.Id);
            editModel.Note.ShouldEqual(order.Note);
            editModel.CustomerFirstName.ShouldEqual(order.Customer.FirstName);
            editModel.CustomerLastName.ShouldEqual(order.Customer.LastName);
            editModel.PaymentInfoOrderedDate.ShouldEqual(order.PaymentInfo.OrderedDate);
            editModel.Type.ShouldEqual(order.Type);
            editModel.OrderDate.ShouldEqual(order.OrderDate);
        }

        [Test]
        public void GetViewModel__Success()
        {
            var viewModel = this.GetService().GetDetailsModel(order.Id);

            viewModel.ShouldNot(Be.Null);
            viewModel.Id.ShouldEqual(order.Id);
            viewModel.Note.ShouldEqual(order.Note);
            viewModel.CustomerFirstName.ShouldEqual(order.Customer.FirstName);
            viewModel.CustomerLastName.ShouldEqual(order.Customer.LastName);
            viewModel.PaymentInfoOrderedDate.ShouldEqual(order.PaymentInfo.OrderedDate);
            viewModel.Type.ShouldEqual(order.Type);
            viewModel.OrderDate.ShouldEqual(order.OrderDate);
            viewModel.PaymentInfoState.ShouldEqual(order.PaymentInfo.State);
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

            int orderId = this.GetService().Create(createModel);

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
    internal class OrdersGridServiceDeletionTests : GridServiceTestsBase<IOrdersCrudService>
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