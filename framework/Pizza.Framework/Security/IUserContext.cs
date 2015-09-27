namespace Pizza.Framework.Security
{
    public interface IUserContext
    {
        IPizzaPrincipal User { get; }
    }
}
