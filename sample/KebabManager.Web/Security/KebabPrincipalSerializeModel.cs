using Pizza.Mvc.Security;

namespace KebabManager.Web.Security
{
    public class KebabPrincipalSerializeModel : PizzaPrincipalSerializeModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}