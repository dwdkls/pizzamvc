using System.Linq;
using NHibernate.Event;
using Pizza.Contracts.Persistence;

namespace Pizza.Framework.Persistence.Audit
{
    public class AuditingEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        private readonly PersistenceModelAuditor auditor;

        public AuditingEventListener(PersistenceModelAuditor auditor)
        {
            this.auditor = auditor;
        }

        public bool OnPreInsert(PreInsertEvent e)
        {
            this.auditor.Insert(this.FindObjectToAudit(e.Entity), e.State, e.Persister);
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent e)
        {
            this.auditor.Update(this.FindObjectToAudit(e.Entity), e.OldState, e.State, e.Persister);
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