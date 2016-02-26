using System.Web.Mvc;

namespace Pizza.Contracts.Default
{
    // TODO: rename?
    public abstract class EditModelBase : IVersionableEditModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public byte[] Version { get; set; }
    }
}