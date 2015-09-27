using System;
using KebabManager.Common.Enums;
using Pizza.Contracts.Default.Presentation;

namespace KebabManager.Contracts.ViewModels.Orders
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