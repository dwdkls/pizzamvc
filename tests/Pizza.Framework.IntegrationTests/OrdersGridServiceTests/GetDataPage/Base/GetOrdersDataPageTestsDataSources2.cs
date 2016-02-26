using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Pizza.Contracts.Presentation.Operations.Requests.Configuration;
using Pizza.Framework.TestTypes.ViewModels.Orders;

namespace Pizza.Framework.IntegrationTests.OrdersGridServiceTests.GetDataPage.Base
{
    internal class GetOrdersDataPageTestsDataSources2
    {
        public readonly Expression<Func<OrderGridModel, object>>[] SortDefinitions = 
        {
            x => x.Note,
            x => x.ItemsCount,
            x => x.TotalPrice,
            x => x.Type,
            x => x.OrderDate,
            x => x.PaymentInfoExternalPaymentId,
            x => x.PaymentInfoNumber,
            x => x.PaymentInfoDouble,
            x => x.PaymentInfoState,
            x => x.PaymentInfoOrderedDate,
            x => x.CustomerLastName,
            x => x.CustomerFingersCount,
            x => x.CustomerHairLength,
            x => x.CustomerType,
            x => x.CustomerPreviousSurgeryDate,
        };

        public IEnumerable<SortMode> SortModes()
        {
            yield return SortMode.Ascending;
            yield return SortMode.Descending;
        }
    }
}