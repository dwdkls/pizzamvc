using Pizza.Contracts.Presentation;

namespace Pizza.Contracts.Security.ViewModels
{
    public class PizzaUserViewModel : IViewModelBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}