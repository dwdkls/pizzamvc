using System;
using System.ComponentModel.DataAnnotations;
using Pizza.Contracts.Default.Presentation;

namespace Pizza.Framework.TestTypes.ViewModels.Customers
{
    public sealed class CustomerEditModel : CreateAndEditModelBase
    {
        [Editable(false)]
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime PreviousSurgeryDate { get; set; }
        public byte[] Version { get; set; }
    }
}