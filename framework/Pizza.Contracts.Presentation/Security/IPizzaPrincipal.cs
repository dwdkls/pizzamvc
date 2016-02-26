using System.Security.Principal;

namespace Pizza.Contracts.Presentation.Security
{
    public interface IPizzaPrincipal : IPrincipal
    {
        int Id { get; set; }
    }
}
