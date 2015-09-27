using System.Collections.Generic;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage.Base;
using Pizza.Framework.TestTypes.Domain.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using System.Linq.Expressions;
using System;
using System.Linq;
using Pizza.Framework.TestTypes.Domain.Common;

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage
{
    [TestFixture]
    internal class FilterByEnum
    {
        [TestFixture]
        internal class FilterByEnumInModel : GetOrdersDataPageTestsBase<OrderType>
        {
            private OrderType filterEnum = OrderType.Home;

            public FilterByEnumInModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.Type, this.filterEnum, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, OrderType>> FilterProperty
            {
                get { return x => x.Type; }
            }

            protected override OrderType FilterValue
            {
                get { return this.filterEnum; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.Type == this.filterEnum;
            }
        }

        [TestFixture]
        internal class FilterByEnumInComponent : GetOrdersDataPageTestsBase<PaymentState>
        {
            PaymentState filterEnum = PaymentState.Cancelled;

            public FilterByEnumInComponent(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {

                var otherEnums = new[] { PaymentState.Completed, PaymentState.Failed, PaymentState.Ordered, PaymentState.Paid };

                return orders.SetFixedValueForSomeItemsAndOtherValuesForOtherItems(
                    x => x.PaymentInfo.State, this.filterEnum, this.expectedTotal, otherEnums);
            }

            protected override Expression<Func<OrderGridModel, PaymentState>> FilterProperty
            {
                get { return x => x.PaymentInfoState; }
            }

            protected override PaymentState FilterValue
            {
                get { return this.filterEnum; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.PaymentInfoState == this.filterEnum;
            }
        }

        [TestFixture]
        internal class FilterByEnumInJoinedModel : GetOrdersDataPageTestsBase<CustomerType>
        {
            CustomerType filterEnum = CustomerType.Government;

            public FilterByEnumInJoinedModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Customer> UpdateCustomersForTest(IEnumerable<Customer> customers)
            {
                var otherEnums = new[] { CustomerType.Enterprise, CustomerType.Individual };

                var temp = customers.SetRandomValuesForAllItems(x => x.Type, otherEnums).ToList();
                temp[0].Type = this.filterEnum;

                return temp;
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                var customerWithCondition = this.CustomersList[0];
                var otherCustomers = this.CustomersList.Skip(1).ToList();

                return orders.SetFixedValueForSomeItemsAndOtherValuesForOtherItems(
                    x => x.Customer, customerWithCondition, this.expectedTotal, otherCustomers);
            }

            protected override Expression<Func<OrderGridModel, CustomerType>> FilterProperty
            {
                get { return x => x.CustomerType; }
            }

            protected override CustomerType FilterValue
            {
                get { return this.filterEnum; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.CustomerType == this.filterEnum;
            }
        }
    }
}