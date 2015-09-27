using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage.Base;
using Pizza.Framework.TestTypes.Domain.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage
{
    [TestFixture]
    internal class FilterByDouble
    {
        private const double filterDouble = int.MaxValue;

        [TestFixture]
        internal class FilterByDoubleInModel : GetOrdersDataPageTestsBase<double>
        {
            public FilterByDoubleInModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.TotalPrice, filterDouble, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, double>> FilterProperty
            {
                get { return x => x.TotalPrice; }
            }

            protected override double FilterValue
            {
                get { return filterDouble; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.TotalPrice == filterDouble;
            }
        }

        [TestFixture]
        internal class FilterByDoubleInComponent : GetOrdersDataPageTestsBase<double>
        {
            public FilterByDoubleInComponent(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetFixedValueForSomeRandomItems(x => x.PaymentInfo.Double, filterDouble, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, double>> FilterProperty
            {
                get { return x => x.PaymentInfoDouble; }
            }

            protected override double FilterValue
            {
                get { return filterDouble; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.PaymentInfoDouble == filterDouble;
            }
        }

        [TestFixture]
        internal class FilterByDoubleInJoinedModel : GetOrdersDataPageTestsBase<double>
        {
            public FilterByDoubleInJoinedModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Customer> UpdateCustomersForTest(IEnumerable<Customer> customers)
            {
                return customers.SetFixedValuesForRangeOfItems(x => x.HairLength, filterDouble, 0, 1);
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

            protected override Expression<Func<OrderGridModel, double>> FilterProperty
            {
                get { return x => x.CustomerHairLength; }
            }

            protected override double FilterValue
            {
                get { return filterDouble; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.CustomerHairLength == filterDouble;
            }
        }
    }
}