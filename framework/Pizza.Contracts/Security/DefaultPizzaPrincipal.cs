using System.Security.Principal;

namespace Pizza.Contracts.Security
{
    public class DefaultPizzaPrincipal : IPizzaPrincipal
    {
        public DefaultPizzaPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public IIdentity Identity { get; }
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