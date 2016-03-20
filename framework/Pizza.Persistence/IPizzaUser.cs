namespace Pizza.Persistence
{
    public interface IPizzaUser : IPersistenceModel
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}