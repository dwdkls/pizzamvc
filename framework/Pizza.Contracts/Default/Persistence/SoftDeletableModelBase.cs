using Pizza.Contracts.Persistence;

namespace Pizza.Contracts.Default.Persistence
{
    public abstract class SoftDeletableModelBase : PersistenceModelBase, ISoftDeletable
    {
        public virtual bool IsDeleted { get; set; }
    }
}