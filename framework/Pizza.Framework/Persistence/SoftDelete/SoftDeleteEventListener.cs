using NHibernate;
using NHibernate.Event;
using Pizza.Contracts.Persistence;

namespace Pizza.Framework.Persistence.SoftDelete
{
    public class SoftDeleteEventListener : IPreDeleteEventListener
    {
        public bool OnPreDelete(PreDeleteEvent preDeleteEvent)
        {
            var softDeletable = preDeleteEvent.Entity as ISoftDeletable;
            if (softDeletable != null)
            {
                // TODO: process all subproperties and delete them if softdeletable?
                // should be normal subproperties deleted?

                var session = preDeleteEvent.Session.GetSession(EntityMode.Poco);

                softDeletable.IsDeleted = true;
                session.Update(softDeletable);
                session.Flush();

                return true;
            }

            return false;
        }
    }
}