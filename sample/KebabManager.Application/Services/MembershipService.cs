using KebabManager.Model.PersistenceModels;
using NHibernate;
using Pizza.Framework.Security;

namespace KebabManager.Application.Services
{
    public class KebabUserService : AuthServiceBase<User>
    {
        public KebabUserService(ISession session) : base(session)
        {
        }

        protected override User GetUser(string username)
        {
            return this.session.QueryOver<User>()
                .Where(x => x.UserName == username)
                .SingleOrDefault();
        }
    }
}