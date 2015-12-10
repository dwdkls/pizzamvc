using Pizza.Contracts.Persistence;

namespace Pizza.Contracts.Security
{
    public interface IPizzaUser : IPersistenceModel
    {
        string UserName { get; set; }
        string Password { get; set; }
    }
}