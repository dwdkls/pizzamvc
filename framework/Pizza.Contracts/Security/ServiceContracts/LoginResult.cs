using Pizza.Contracts.Security.ViewModels;

namespace Pizza.Contracts.Security.ServiceContracts
{
    public class LoginResult<TUserViewModel>
        where TUserViewModel : PizzaUserViewModel
    {
        public bool Succeed
        {
            get { return this.User != null; }
        }

        public TUserViewModel User { get; }

        public LoginResult()
        {
        }

        public LoginResult(TUserViewModel user)
        {
            this.User = user;
        }
    }
}