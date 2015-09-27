using System;
using Pizza.Contracts.Default.Presentation;
using Pizza.Framework.TestTypes.Domain.Common;

namespace Pizza.Framework.TestTypes.ViewModels.Orders
{
    public sealed class OrderDetailsModel : DetailsModelBase
    {
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime PaymentInfoOrderedDate { get; set; }
        public PaymentState PaymentInfoState { get; set; }
        public OrderType Type { get; set; }
        public string Note { get; set; }
        public string Prescription { get; set; }
    }
}