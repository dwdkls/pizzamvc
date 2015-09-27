using System.Security.Principal;

namespace Pizza.Framework.Security
{
    public interface IPizzaPrincipal : IPrincipal
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
