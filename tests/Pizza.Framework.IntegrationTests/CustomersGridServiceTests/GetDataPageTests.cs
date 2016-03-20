using System;
using NUnit.Framework;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Requests.Configuration;
using Pizza.Framework.IntegrationTests.Base;
using Pizza.Framework.IntegrationTests.TestServices;
using Pizza.Framework.DataGeneration;
using Pizza.Framework.IntegrationTests.Base.Helpers;
using Pizza.Framework.IntegrationTests.Extensions;
using Pizza.Framework.TestTypes.Model.PersistenceModels;
using Pizza.Framework.TestTypes.ViewModels.Customers;
using Ploeh.AutoFixture;

namespace Pizza.Framework.IntegrationTests.CustomersGridServiceTests
{
    internal class GetDataPageTests : GridServiceTestsBase<ICustomersCrudService>
    {
        private readonly string testName = "testABCDEFGHtests";
        private readonly int testInt = 12345;
        private readonly DateTime testDate = new DateTime(1999, 11, 19);

        private readonly int totalCustomers = 27;
        private readonly SortConfiguration sort = GridConfigurationHelper.GetSort<CustomerGridModel>(x => x.FirstName);

        protected override void PrepareFixture()
        {
            this.RecreateDatabase();

            var customers = this.fixture.CreateMany<Customer>(this.totalCustomers)
                .SetFixedValuesForRangeOfItems(p => p.LastName, this.testName, 7, 8)
                .SetFixedValuesForRangeOfItems(p => p.FingersCount, this.testInt, 11, 8)
                .SetFixedValuesForRangeOfItems(p => p.PreviousSurgeryDate, this.testDate, 12, 8);

            this.nhSessionHelper.SaveManyInNewSession(customers);
        }

        public void WithoutFilter_PageSizeGreaterThanTotalItemsCount()
        {
            var filter = FilterConfiguration<CustomerGridModel>.Empty;
            this.RunGetDataPageTest(pageNumber: 1, pageSize: 50, expectedLoaded: 27, expectedTotal: 27, filter: filter);
        }

        [TestCase(1, 5)]
        [TestCase(2, 5)]
        [TestCase(6, 2)]
        public void WithoutFilter(int pageNumber, int loadedItemsCount)
        {
            var filter = FilterConfiguration<CustomerGridModel>.Empty;
            this.RunGetDataPageTest(pageNumber: pageNumber, pageSize: 5, expectedLoaded: loadedItemsCount, expectedTotal: 27, filter: filter);
        }

        [TestCase(1, 5)]
        [TestCase(2, 3)]
        public void FilteredByString(int pageNumber, int loadedItemsCount)
        {
            var filter = GridConfigurationHelper.GetFilter<CustomerGridModel, string>(x => x.LastName, "ABCDEFGH");
            this.RunGetDataPageTest(pageNumber: pageNumber, pageSize: 5, expectedLoaded: loadedItemsCount, expectedTotal: 8, filter: filter);
        }

        [TestCase(1, 5)]
        [TestCase(2, 3)]
        public void FilteredByInt(int pageNumber, int loadedItemsCount)
        {
            var filter = GridConfigurationHelper.GetFilter<CustomerGridModel, int>(x => x.FingersCount, this.testInt);
            this.RunGetDataPageTest(pageNumber: pageNumber, pageSize: 5, expectedLoaded: loadedItemsCount, expectedTotal: 8, filter: filter);
        }

        [TestCase(1, 5)]
        [TestCase(2, 3)]
        public void FilteredByDate(int pageNumber, int loadedItemsCount)
        {
            var filter = GridConfigurationHelper.GetFilter<CustomerGridModel, DateTime>(x => x.PreviousSurgeryDate, this.testDate);
            this.RunGetDataPageTest(pageNumber: pageNumber, pageSize: 5, expectedLoaded: loadedItemsCount, expectedTotal: 8, filter: filter);
        }

        private void RunGetDataPageTest(int pageNumber, int pageSize, int expectedLoaded, int expectedTotal, 
            FilterConfiguration<CustomerGridModel> filter)
        {
            var request = new DataRequest<CustomerGridModel>(pageNumber, pageSize, this.sort, filter);

            var dataPage = this.GetService().GetDataPage(request);

            dataPage.ShouldBeValid(pageNumber, pageSize, expectedLoaded, expectedTotal);
        }
    }
}