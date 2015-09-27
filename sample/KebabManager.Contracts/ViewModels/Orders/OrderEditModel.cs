using System;
using System.ComponentModel.DataAnnotations;
using KebabManager.Contracts.Common;
using Pizza.Contracts.Default.Presentation;

namespace KebabManager.Contracts.ViewModels.Orders
{
    public sealed class OrderEditModel : EditModelBase
    {
        [Editable(false)]
        public string CustomerFirstName { get; set; }

        [Editable(false)]
        public string CustomerLastName { get; set; }

        [Editable(false)]
        public DateTime PaymentInfoOrderedDate { get; set; }

        [Editable(false)]
        public DateTime OrderDate { get; set; }

        [Editable(false)]
        public OrderType Type { get; set; }

        public string Note { get; set; }
    }
}