using System;
using Pizza.Contracts.Default.Presentation;

namespace Pizza.Framework.TestTypes.ViewModels.Customers
{
    public sealed class CustomerCreateModel : CreateModelBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PreviousSurgeryDate { get; set; }
    }
}