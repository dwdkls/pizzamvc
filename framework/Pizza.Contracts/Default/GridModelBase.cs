using System.Web.Mvc;

namespace Pizza.Contracts.Default
{
    public abstract class GridModelBase : IGridModelBase
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public override string ToString()
        {
            return $"Object of type: '{this.GetType().Name}' with ID: '{this.Id}'";
        }
    }
}