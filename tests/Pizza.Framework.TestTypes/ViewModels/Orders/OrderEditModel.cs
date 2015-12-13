using System;
using System.ComponentModel.DataAnnotations;
using Pizza.Contracts.Presentation.Default;
using Pizza.Framework.TestTypes.Model.Common;

namespace Pizza.Framework.TestTypes.ViewModels.Orders
{
    public sealed class OrderEditModel : CreateAndEditModelBase
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