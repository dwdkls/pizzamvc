using System;
using Pizza.Contracts.Default;

namespace Pizza.Framework.TestTypes.ViewModels.Customers
{
    public sealed class CustomerDetailsModel : DetailsModelBase
    {
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PreviousSurgeryDate { get; set; }
    }
}