namespace Pizza.Persistence.Default
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
            return $"Object of type: '{this.GetType().Name}' with ID: '{this.Id}'";
        }
    }
}