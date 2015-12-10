using System.Linq;
using Autofac;
using NHibernate.Event;
using Pizza.Contracts.Persistence;

namespace Pizza.Framework.Persistence.Audit
{
    public class AuditingEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        private static PersistenceModelAuditor Auditor
        {
            get
            {
                // TODO: this is not nice hack, find way to get access to AutofacContainer from NHibernate
                var persistenceModelAuditor = PizzaServerContext.Current.Container.Resolve<PersistenceModelAuditor>();
                return persistenceModelAuditor;
            }
        }

        public bool OnPreInsert(PreInsertEvent e)
        {
            Auditor.Insert(this.FindObjectToAudit(e.Entity), e.State, e.Persister);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent e)
        {
            Auditor.Update(this.FindObjectToAudit(e.Entity), e.OldState, e.State, e.Persister);
            return false;
        }

        private IAuditable FindObjectToAudit(object persistenceModel)
        {
            IAuditable auditable = null;

            var auditableProperty = persistenceModel.GetType().GetProperties()
                .SingleOrDefault(x => typeof(IAuditable).IsAssignableFrom(x.PropertyType));

            if (auditableProperty != null)
            {
                auditable = (IAuditable)auditableProperty.GetValue(persistenceModel, null);
            }

            return auditable;
        }
    }
}