namespace Pizza.Contracts.Presentation.Security
{
    public interface IPizzaUserContext
    {
        IPizzaPrincipal CurrentUser { get; }
    }
}
