namespace Pizza.Persistence
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; } 
    }
}