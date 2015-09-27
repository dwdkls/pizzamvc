using Pizza.Contracts.Persistence;

namespace Pizza.Contracts.Default.Persistence
{
    public abstract class PersistenceModelBase : IPersistenceModel, IVersionable
    {
        protected PersistenceModelBase()
        {
            this.AuditInfo = new AuditInfo();
        }

        public virtual AuditInfo AuditInfo { get; set; }
        public virtual int Id { get; set; }
        public virtual byte[] Version { get; set; }

        public override string ToString()
        {
            return string.Format("Object of type: '{0}' with ID: '{1}'", this.GetType().Name, this.Id);
        }
    }
}