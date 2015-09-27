using System.Collections.Generic;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage.Base;
using Pizza.Framework.TestTypes.Domain.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage
{
    [TestFixture]
    internal class FilterByInt
    {
        private const int filterInt = int.MaxValue;

        [TestFixture]
        internal class FilterByIntInModel : GetOrdersDataPageTestsBase<int>
        {
            public FilterByIntInModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.ItemsCount, filterInt, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, int>> FilterProperty
            {
                get { return x => x.ItemsCount; }
            }

            protected override int FilterValue
            {
                get { return filterInt; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.ItemsCount == filterInt;
            }
        }

        [TestFixture]
        internal class FilterByIntInComponent : GetOrdersDataPageTestsBase<int>
        {
            public FilterByIntInComponent(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.PaymentInfo.Number, filterInt, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, int>> FilterProperty
            {
                get { return x => x.PaymentInfoNumber; }
            }

            protected override int FilterValue
            {
                get { return filterInt; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.PaymentInfoNumber == filterInt;
            }
        }

        [TestFixture]
        internal class FilterByIntInJoinedModel : GetOrdersDataPageTestsBase<int>
        {
            public FilterByIntInJoinedModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Customer> UpdateCustomersForTest(IEnumerable<Customer> customers)
            {
                return customers.SetFixedValuesForRangeOfItems(x => x.FingersCount, filterInt, 0, 1);
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                var customerWithCondition = this.CustomersList[0];
                var otherCustomers = this.CustomersList.Skip(1).ToList();

                return orders
                    .SetFixedValueForSomeItemsAndOtherValuesForOtherItems(
                        x => x.Customer, customerWithCondition, this.expectedTotal, otherCustomers)
                    .ToList();
            }

            protected override Expression<Func<OrderGridModel, int>> FilterProperty
            {
                get { return x => x.CustomerFingersCount; }
            }

            protected override int FilterValue
            {
                get { return filterInt; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.CustomerFingersCount == filterInt;
            }
        }
    }
}