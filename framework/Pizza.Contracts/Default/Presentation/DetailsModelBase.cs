using System.Web.Mvc;
using Pizza.Contracts.Presentation;

namespace Pizza.Contracts.Default.Presentation
{
    public abstract class DetailsModelBase : IDetailsModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}