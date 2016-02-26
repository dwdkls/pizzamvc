using System.Linq;
using NUnit.Asserts.Compare;
using NUnit.Framework;
using Pizza.Contracts.Operations.Results;

namespace Pizza.Framework.IntegrationTests.Extensions
{
    internal static class TestExtensions
    {
        public static void ShouldBeValid<TGridModel>(this DataPageResult<TGridModel> dataPage, 
            int pageNumber, int pageSize, int expectedLoaded, int expectedTotal)
        {
            dataPage.Should(Be.Not.Null);
            dataPage.Items.Should(Be.Not.Empty);
            dataPage.Items.Count.Should(Be.EqualTo(expectedLoaded));
            dataPage.PagingInfo.Should(Compares.To(new PagingInfo(pageNumber, pageSize, expectedTotal)));
        }
    }
}