namespace Pizza.Persistence.Default
{
    public abstract class SoftDeletableModelBase : PersistenceModelBase, ISoftDeletable
    {
        public virtual bool IsDeleted { get; set; }
    }
}