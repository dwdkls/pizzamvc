namespace Pizza.Contracts.Presentation.Security.ViewModels
{
    public class PizzaUserViewModel : IViewModelBase
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }
}