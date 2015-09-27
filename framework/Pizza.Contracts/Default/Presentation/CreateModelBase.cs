using System.Web.Mvc;
using Pizza.Contracts.Presentation;

namespace Pizza.Contracts.Default.Presentation
{
    public abstract class CreateModelBase : ICreateModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
    }
}