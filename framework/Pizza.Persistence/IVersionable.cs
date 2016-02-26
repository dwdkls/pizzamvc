namespace Pizza.Persistence
{
    public interface IVersionable 
    {
        byte[] Version { get; set; }
    }
}