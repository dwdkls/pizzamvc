using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.Base;
using Pizza.Framework.IntegrationTests.Base.Helpers;
using Pizza.Framework.IntegrationTests.Extensions;
using Pizza.Framework.IntegrationTests.TestServices;
using Pizza.Framework.TestTypes.Model.Common;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Orders;
using Pizza.Utils;
using Ploeh.AutoFixture;

namespace Pizza.Framework.IntegrationTests.OrdersCrudServiceTests.GetDataPage.Base
{
    [TestFixture(8, 19, Description = "more than one page satisfy filter condition")]
    [TestFixture(6, 6, Description = "only few records (less than one page) satisfy filter condition")]
    internal abstract class GetOrdersDataPageTestsBase<TFilter> : CrudServiceTestsBase<IOrdersCrudService>
    {
        protected List<Customer> CustomersList { get; private set; }
        protected List<Order> OrdersList { get; private set; }

        private readonly int pageNumber = 1;
        private readonly int pageSize = 8;

        // these have to be protected
        protected readonly int expectedLoaded;
        protected readonly int expectedTotal;

        protected GetOrdersDataPageTestsBase(int expectedLoaded, int expectedTotal)
        {
            this.expectedLoaded = expectedLoaded;
            this.expectedTotal = expectedTotal;
        }

        protected sealed override void PrepareFixture()
        {
            this.RecreateDatabase();

            this.CustomersList = this.CreateCustomers();
            this.OrdersList = this.CreateOrders();

            this.nhSessionHelper.SaveManyInNewSession(this.CustomersList);
            this.nhSessionHelper.SaveManyInNewSession(this.OrdersList);
        }

        protected abstract Expression<Func<OrderGridModel, TFilter>> FilterProperty { get; }
        protected abstract TFilter FilterValue { get; }
        protected abstract bool ValidateModelCondition(OrderGridModel model);

        [Test]
        public void GetData(
            [ValueSource(typeof(GetOrdersDataPageTestsDataSources2), "SortDefinitions")]  Expression<Func<OrderGridModel, object>> sortExpression,
            [ValueSource(typeof(GetOrdersDataPageTestsDataSources2), "SortModes")] SortMode sortMode)
        {
            var dataPage = this.GetDataPage(GridConfigurationHelper.GetSort(sortExpression, sortMode));

            this.AssertPageWithOrder(dataPage, sortExpression, sortMode);
        }

        #region Fixture preparation

        private List<Customer> CreateCustomers()
        {
            const int customerCount = 5;
            var customersTemp = this.fixture.Build<Customer>()
                .CreateMany(customerCount);

            var customersList = this.UpdateCustomersForTest(customersTemp).ToList();

            if (customersList.Count != customerCount)
            {
                throw new InvalidOperationException("Number of Customers must not be changed in specific fixture");
            }

            return customersList;
        }

        private List<Order> CreateOrders()
        {
            const int orderCount = 27;

            var ordersTemp = this.fixture.Build<Order>()
                .With(x => x.Type, OrderType.Standard)
                .CreateMany(orderCount)
                .SetRandomValuesForAllItems(v => v.Customer, this.CustomersList)
                .ToList();

            var ordersList = this.UpdateOrdersForTest(ordersTemp).ToList();

            if (ordersList.Count != orderCount)
            {
                throw new InvalidOperationException("Number of Orders must not be changed in specific fixture");
            }

            return ordersList;
        }

        protected virtual IEnumerable<Customer> UpdateCustomersForTest(IEnumerable<Customer> customers)
        {
            return customers;
        }

        protected virtual IEnumerable<Order> UpdateOrdersForTest(IEnumerable<Order> orders)
        {
            return orders;
        }

        #endregion

        private DataPageResult<OrderGridModel> GetDataPage(SortConfiguration sortConfiguration)
        {
            var filterConfiguration = GridConfigurationHelper.GetFilter(this.FilterProperty, this.FilterValue);

            var request = new DataRequest<OrderGridModel>(this.pageNumber, this.pageSize, sortConfiguration, filterConfiguration);
            var dataPage = this.GetService().GetDataPage(request);

            return dataPage;
        }

        private void AssertPageWithOrder(DataPageResult<OrderGridModel> dataPage,
            Expression<Func<OrderGridModel, object>> sortProperty, SortMode sortMode)
        {
            var propertyName = ObjectHelper.GetPropertyName(sortProperty);

            dataPage.ShouldBeValid(this.pageNumber, this.pageSize, this.expectedLoaded, this.expectedTotal);
            dataPage.Items.Select(this.ValidateModelCondition).Should(Have.All.True);

            if (sortMode == SortMode.Ascending)
            {
                dataPage.Items.Should(Be.Ordered.By(propertyName));
            }
            else if (sortMode == SortMode.Descending)
            {
                dataPage.Items.Should(Be.Ordered.By(propertyName).Descending);
            }
        }
    }
}