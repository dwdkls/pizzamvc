using System.Web.Mvc;

namespace Pizza.Contracts.Default
{
    // TODO: thus optimistic concurrency, this class makes no sense
    public abstract class CreateAndEditModelBase : IEditModelBase, ICreateModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}