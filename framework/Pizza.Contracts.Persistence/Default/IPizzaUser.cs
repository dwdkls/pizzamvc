namespace Pizza.Contracts.Persistence.Default
{
    public interface IPizzaUser : IPersistenceModel
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}