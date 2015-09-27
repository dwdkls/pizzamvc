using System;
using System.ComponentModel.DataAnnotations;
using KebabManager.Common.Enums;
using Pizza.Contracts.Default.Presentation;

namespace KebabManager.Contracts.ViewModels.Customers
{
    public sealed class CustomerDetailsModel : DetailsModelBase
    {
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Fingers count")]
        public int FingersCount { get; set; }

        [Display(Name = "Previous surgery date")]
        public DateTime PreviousSurgeryDate { get; set; }

        [Display(Name = "Customer type")]
        public CustomerType Type { get; set; }
    }
}