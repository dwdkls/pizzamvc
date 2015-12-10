using System;
using Pizza.Contracts.Default.Presentation;
using Pizza.Framework.TestTypes.Model.Common;

namespace Pizza.Framework.TestTypes.ViewModels.Orders
{
    public sealed class OrderCreateModel : CreateModelBase
    {
        public int CustomerId { get; set; }
        public OrderType Type { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentInfoOrderedDate { get; set; }
        public int Duration { get; set; }
        public string Note { get; set; }
    }
}