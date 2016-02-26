using System.Web;
using Pizza.Contracts.Security;
using Pizza.Framework.Security;

namespace Pizza.Mvc.Security
{
    public class DefaultApplicationUserContext : IPizzaUserContext
    {
        public IPizzaPrincipal CurrentUser
        {
            get { return (IPizzaPrincipal)HttpContext.Current.User; }
        }
    }
}