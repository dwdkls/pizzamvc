using System.ComponentModel.DataAnnotations;

namespace Pizza.Contracts.Security.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Field {0} is required")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Field {0} is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}