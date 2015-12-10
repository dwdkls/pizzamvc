using System;
using System.ComponentModel.DataAnnotations;
using Pizza.Contracts.Default.Presentation;
using Pizza.Framework.TestTypes.Model.Common;

namespace Pizza.Framework.TestTypes.ViewModels.Orders
{
    public sealed class OrderGridModel : GridModelBase
    {
        [Display(Name = "Order date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Type")]
        public OrderType Type { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Items Count")]
        public int ItemsCount { get; set; }

        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }


        [Display(Name = "Ordered date")]
        public DateTime PaymentInfoOrderedDate { get; set; }

        [Display(Name = "State")]
        public PaymentState PaymentInfoState { get; set; }

        public int PaymentInfoNumber { get; set; }

        public double PaymentInfoDouble { get; set; }

        public string PaymentInfoExternalPaymentId { get; set; }


        [Display(Name = "First name")]
        public string CustomerFirstName { get; set; }

        [Display(Name = "Last name")]
        public string CustomerLastName { get; set; }

        public int CustomerFingersCount { get; set; }
        public double CustomerHairLength { get; set; }
        public DateTime CustomerPreviousSurgeryDate { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}