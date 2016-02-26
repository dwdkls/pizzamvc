namespace Pizza.Contracts.Security
{
    public interface IPizzaUserContext
    {
        IPizzaPrincipal CurrentUser { get; }
    }
}
