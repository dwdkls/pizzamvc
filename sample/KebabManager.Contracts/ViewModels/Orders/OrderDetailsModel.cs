using System;
using KebabManager.Common.Enums;
using Pizza.Contracts.Default;

namespace KebabManager.Contracts.ViewModels.Orders
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