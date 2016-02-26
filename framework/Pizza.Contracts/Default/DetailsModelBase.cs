using System.Web.Mvc;

namespace Pizza.Contracts.Default
{
    public abstract class DetailsModelBase : IDetailsModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}