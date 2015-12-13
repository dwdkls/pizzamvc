using Pizza.Contracts.Presentation.Security.ViewModels;

namespace Pizza.Contracts.Presentation.Security.ServiceContracts
{
    public class LoginResult<TUserViewModel>
        where TUserViewModel : PizzaUserViewModel
    {
        public bool Succeed
        {
            get { return this.User != null; }
        }

        public TUserViewModel User { get; private set; }

        public LoginResult()
        {
        }

        public LoginResult(TUserViewModel user)
        {
            this.User = user;
        }
    }
}