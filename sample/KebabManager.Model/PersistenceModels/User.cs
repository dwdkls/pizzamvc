using Pizza.Contracts.Persistence.Attributes;
using Pizza.Contracts.Persistence.Default;

namespace KebabManager.Model.PersistenceModels
{
    public class User : PersistenceModelBase, IPizzaUser
    {
        [Unique, UnicodeString(30)]
        public virtual string UserName { get; set; }
        [FixedLengthAnsiString(128)]
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
    }
}
