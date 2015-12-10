using System.Security.Principal;

namespace Pizza.Framework.Security
{
    public class DefaultPizzaPrincipal : IPizzaPrincipal
    {
        public DefaultPizzaPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public IIdentity Identity { get; private set; }
        public int Id { get; set; }

        public bool IsInRole(string role)
        {
            return false;
        }

        public override string ToString()
        {
            return this.Identity.Name;
        }
    }
}