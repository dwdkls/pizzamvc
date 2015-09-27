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
    internal class FilterByDateTime
    {
        private static readonly DateTime filterDate = new DateTime(1919, 11, 19);

        [TestFixture]
        internal class FilterByDateTimeInModel : GetOrdersDataPageTestsBase<DateTime>
        {
            public FilterByDateTimeInModel(int expectedLoaded, int expectedTotal) : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.OrderDate, filterDate, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, DateTime>> FilterProperty
            {
                get { return x => x.OrderDate; }
            }

            protected override DateTime FilterValue
            {
                get { return filterDate; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.OrderDate == filterDate;
            }
        }

        [TestFixture]
        internal class FilterByDateTimeInComponent : GetOrdersDataPageTestsBase<DateTime>
        {
            public FilterByDateTimeInComponent(int expectedLoaded, int expectedTotal) : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.PaymentInfo.OrderedDate, filterDate, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, DateTime>> FilterProperty
            {
                get { return x => x.PaymentInfoOrderedDate; }
            }

            protected override DateTime FilterValue
            {
                get { return filterDate; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.PaymentInfoOrderedDate == filterDate;
            }
        }

        [TestFixture]
        internal class FilterByDateTimeInJoinedModel : GetOrdersDataPageTestsBase<DateTime>
        {
            public FilterByDateTimeInJoinedModel(int expectedLoaded, int expectedTotal) : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Customer> UpdateCustomersForTest(IEnumerable<Customer> customers)
            {
                return customers
                    .SetFixedValuesForRangeOfItems(x => x.PreviousSurgeryDate, filterDate, 0, 1)
                    .ToList();
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

            protected override Expression<Func<OrderGridModel, DateTime>> FilterProperty
            {
                get { return x => x.CustomerPreviousSurgeryDate; }
            }

            protected override DateTime FilterValue
            {
                get { return filterDate; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.CustomerPreviousSurgeryDate == filterDate;
            }
        }
    }
}