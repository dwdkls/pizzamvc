namespace Pizza.Contracts.Persistence
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; } 
    }
}