using System.Security.Principal;

namespace Pizza.Contracts.Security
{
    public interface IPizzaPrincipal : IPrincipal
    {
        int Id { get; set; }
    }
}
