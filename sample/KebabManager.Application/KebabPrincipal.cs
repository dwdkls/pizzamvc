using Pizza.Contracts.Security;

namespace KebabManager.Application
{
    public class KebabPrincipal : DefaultPizzaPrincipal
    {
        public KebabPrincipal(string username) : base(username)
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName}  {this.LastName}";
        }
    }
}