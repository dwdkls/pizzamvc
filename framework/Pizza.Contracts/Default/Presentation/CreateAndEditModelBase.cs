using System.Web.Mvc;
using Pizza.Contracts.Presentation;

namespace Pizza.Contracts.Default.Presentation
{
    // TODO: thus optimistic concurrency, this class makes no sense
    public abstract class CreateAndEditModelBase : IEditModelBase, ICreateModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}