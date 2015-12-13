using System.Web.Mvc;

namespace Pizza.Contracts.Presentation.Default
{
    public abstract class DetailsModelBase : IDetailsModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}