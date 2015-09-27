using System;
using System.Linq.Expressions;
using Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage.Base;
using Pizza.Framework.TestTypes.ViewModels.Orders;

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage
{
    internal class NoFilter : GetOrdersDataPageTestsBase<object>
    {
        public NoFilter(int expectedLoaded, int expectedTotal) : base(8, 27)
        {
        }

        protected override Expression<Func<OrderGridModel, object>> FilterProperty
        {
            get { return null; }
        }

        protected override object FilterValue
        {
            get { return null; }
        }

        protected override bool ValidateModelCondition(OrderGridModel model)
        {
            return true;
        }
    }
}