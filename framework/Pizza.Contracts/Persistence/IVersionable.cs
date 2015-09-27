namespace Pizza.Contracts.Persistence
{
    public interface IVersionable 
    {
        byte[] Version { get; set; }
    }
}