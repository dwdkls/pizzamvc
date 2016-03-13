using System.Web;
using Pizza.Contracts.Security;

namespace Pizza.Mvc.Security
{
    public class DefaultApplicationUserContext : IPizzaUserContext
    {
        public IPizzaPrincipal CurrentUser
        {
            get { return HttpContext.Current.User as IPizzaPrincipal; }
        }
    }
}