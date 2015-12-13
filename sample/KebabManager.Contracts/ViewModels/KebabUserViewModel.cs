using Pizza.Contracts.Presentation.Security.ViewModels;

namespace KebabManager.Contracts.ViewModels
{
    public class KebabUserViewModel : PizzaUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}