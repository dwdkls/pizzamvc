using System.Security.Principal;

namespace Pizza.Framework.Security
{
    public interface IPizzaPrincipal : IPrincipal
    {
        int Id { get; set; }
    }
}
