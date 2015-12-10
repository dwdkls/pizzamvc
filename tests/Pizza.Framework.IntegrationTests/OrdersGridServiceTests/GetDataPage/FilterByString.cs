using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage.Base;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using System.Linq.Expressions;

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage
{
    [TestFixture]
    internal class FilterByString
    {
        private const string filterString = "java is awesome";

        static readonly Func<string> stringGenerator = () => Guid.NewGuid() + filterString + Guid.NewGuid();

        [TestFixture]
        internal class FilterByStringInModel : GetOrdersDataPageTestsBase<string>
        {
            public FilterByStringInModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetGeneratedValuesForSomeRandomItems(x => x.Note, stringGenerator, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, string>> FilterProperty
            {
                get { return x => x.Note; }
            }

            protected override string FilterValue
            {
                get { return filterString; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.Note.Contains(filterString);
            }
        }

        [TestFixture]
        internal class FilterByStringInComponent : GetOrdersDataPageTestsBase<string>
        {
            public FilterByStringInComponent(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
            {
                return orders.SetGeneratedValuesForSomeRandomItems(x => x.PaymentInfo.ExternalPaymentId, stringGenerator, this.expectedTotal);
            }

            protected override Expression<Func<OrderGridModel, string>> FilterProperty
            {
                get { return x => x.PaymentInfoExternalPaymentId; }
            }

            protected override string FilterValue
            {
                get { return filterString; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.PaymentInfoExternalPaymentId.Contains(filterString);
            }
        }

        [TestFixture]
        internal class FilterByStringInJoinedModel : GetOrdersDataPageTestsBase<string>
        {
            public FilterByStringInJoinedModel(int expectedLoaded, int expectedTotal)
                : base(expectedLoaded, expectedTotal)
            {
            }

            protected override IEnumerable<Customer> UpdateCustomersForTest(IEnumerable<Customer> customers)
            {
                return customers.SetGeneratedValuesForRangeOfItems(x => x.LastName, stringGenerator, 0, 1);
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

            protected override Expression<Func<OrderGridModel, string>> FilterProperty
            {
                get { return x => x.CustomerLastName; }
            }

            protected override string FilterValue
            {
                get { return filterString; }
            }

            protected override bool ValidateModelCondition(OrderGridModel model)
            {
                return model.CustomerLastName.Contains(filterString);
            }
        }
    }
}