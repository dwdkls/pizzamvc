using System;
using System.Linq;
using NHibernate.Persister.Entity;
using Pizza.Contracts.Security;
using Pizza.Persistence;

namespace Pizza.Framework.Persistence.Audit
{
    public class PersistenceModelAuditor
    {
        private readonly IPizzaUserContext pizzaUserContext;

        public PersistenceModelAuditor(IPizzaUserContext pizzaUserContext)
        {
            this.pizzaUserContext = pizzaUserContext;
        }

        public void Insert(IAuditable auditable, object[] state, IEntityPersister persister)
        {
            if (auditable == null)
                return;

            this.SetCreate(auditable, state);
            this.SetChange(auditable, state);
        }

        public void Update(IAuditable auditable, object[] oldState, object[] state, IEntityPersister persister)
        {
            if (auditable == null)
                return;

            this.SetChange(auditable, state);
        }

        private void SetCreate(IAuditable auditable, object[] state)
        {
            auditable.CreatedBy = this.GetUserId();
            auditable.CreatedTime = DateTime.Now;

            this.SetState(state, nameof(IAuditable.CreatedBy), auditable.CreatedBy);
            this.SetState(state, nameof(IAuditable.CreatedTime), auditable.CreatedTime);
        }

        private void SetChange(IAuditable auditable, object[] state)
        {
            auditable.ChangedBy = this.GetUserId();
            auditable.ChangedTime = DateTime.Now;

            this.SetState(state, nameof(IAuditable.ChangedBy), auditable.ChangedBy);
            this.SetState(state, nameof(IAuditable.ChangedTime), auditable.ChangedTime);
        }

        private void SetState(object[] state, string propertyName, object value)
        {
            var auditableModel = state.SingleOrDefault(x => x is IAuditable);
            if (auditableModel == null)
                return;

            var property = typeof(IAuditable).GetProperty(propertyName);
            property.SetValue(auditableModel, value, null);
        }

        private int GetUserId()
        {
            var user = this.pizzaUserContext.CurrentUser;

            return user != null ? user.Id : 0;
        }
    }
}