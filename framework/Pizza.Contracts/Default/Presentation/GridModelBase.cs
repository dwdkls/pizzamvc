using System.Web.Mvc;
using Pizza.Contracts.Presentation;

namespace Pizza.Contracts.Default.Presentation
{
    public abstract class GridModelBase : IGridModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public override string ToString()
        {
            return string.Format("Object of type: '{0}' with ID: '{1}'", this.GetType().Name, this.Id);
        }
    }
}