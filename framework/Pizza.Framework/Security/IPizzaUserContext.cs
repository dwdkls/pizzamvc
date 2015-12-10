namespace Pizza.Framework.Security
{
    public interface IPizzaUserContext
    {
        IPizzaPrincipal CurrentUser { get; }
    }
}
