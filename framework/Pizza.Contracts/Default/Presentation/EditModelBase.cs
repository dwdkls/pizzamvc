using System.Web.Mvc;
using Pizza.Contracts.Presentation;

namespace Pizza.Contracts.Default.Presentation
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