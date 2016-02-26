namespace Pizza.Contracts
{
    public interface IVersionableEditModelBase : IEditModelBase
    {
        byte[] Version { get; set; }
    }
}